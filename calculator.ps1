# Motor Premium Calculator - PowerShell Implementation
# A simple PowerShell script to demonstrate the motor premiums calculator

param(
    [string]$ConfigFile = "motor_premiums_calculator.json",
    [switch]$Example,
    [switch]$ListOptions
)

function Get-CalculatorConfig {
    param([string]$ConfigPath)
    
    if (-not (Test-Path $ConfigPath)) {
        throw "Configuration file not found: $ConfigPath"
    }
    
    $configContent = Get-Content $ConfigPath -Raw | ConvertFrom-Json
    return $configContent.motor_premiums_calculator
}

function Get-Multiplier {
    param(
        [object]$FactorConfig,
        [string]$Value
    )
    
    if ($FactorConfig.PSObject.Properties.Name -contains "options" -and $FactorConfig.options.PSObject.Properties.Name -contains $Value) {
        return $FactorConfig.options.$Value.multiplier
    }
    elseif ($FactorConfig.PSObject.Properties.Name -contains "ranges" -and $FactorConfig.ranges.PSObject.Properties.Name -contains $Value) {
        return $FactorConfig.ranges.$Value.multiplier
    }
    elseif ($FactorConfig.PSObject.Properties.Name -contains "amounts" -and $FactorConfig.amounts.PSObject.Properties.Name -contains $Value) {
        return $FactorConfig.amounts.$Value.multiplier
    }
    elseif ($Value -eq "enabled" -and $FactorConfig.PSObject.Properties.Name -contains "enabled") {
        return $FactorConfig.enabled.multiplier
    }
    
    return 1.0
}

function Calculate-Premium {
    param(
        [object]$Config,
        [object]$CustomerData
    )
    
    $premium = $Config.base_premium.amount
    $multipliers = @{}
    
    # Apply vehicle factors
    if ($CustomerData.vehicle) {
        foreach ($factorName in $Config.vehicle_factors.PSObject.Properties.Name) {
            if ($CustomerData.vehicle.PSObject.Properties.Name -contains $factorName) {
                $value = $CustomerData.vehicle.$factorName
                if ($value) {
                    $multiplier = Get-Multiplier -FactorConfig $Config.vehicle_factors.$factorName -Value $value
                    $premium *= $multiplier
                    $multipliers[$factorName] = $multiplier
                }
            }
        }
    }
    
    # Apply driver factors
    if ($CustomerData.driver) {
        foreach ($factorName in $Config.driver_factors.PSObject.Properties.Name) {
            if ($CustomerData.driver.PSObject.Properties.Name -contains $factorName) {
                $value = $CustomerData.driver.$factorName
                if ($value) {
                    $multiplier = Get-Multiplier -FactorConfig $Config.driver_factors.$factorName -Value $value
                    $premium *= $multiplier
                    $multipliers[$factorName] = $multiplier
                }
            }
        }
    }
    
    # Apply location factors
    if ($CustomerData.location) {
        foreach ($factorName in $Config.location_factors.PSObject.Properties.Name) {
            if ($CustomerData.location.PSObject.Properties.Name -contains $factorName) {
                $value = $CustomerData.location.$factorName
                if ($value) {
                    $multiplier = Get-Multiplier -FactorConfig $Config.location_factors.$factorName -Value $value
                    $premium *= $multiplier
                    $multipliers[$factorName] = $multiplier
                }
            }
        }
    }
    
    # Apply usage factors
    if ($CustomerData.usage) {
        foreach ($factorName in $Config.usage_factors.PSObject.Properties.Name) {
            if ($CustomerData.usage.PSObject.Properties.Name -contains $factorName) {
                $value = $CustomerData.usage.$factorName
                if ($value) {
                    $multiplier = Get-Multiplier -FactorConfig $Config.usage_factors.$factorName -Value $value
                    $premium *= $multiplier
                    $multipliers[$factorName] = $multiplier
                }
            }
        }
    }
    
    # Apply coverage options
    if ($CustomerData.coverage) {
        foreach ($factorName in $Config.coverage_options.PSObject.Properties.Name) {
            if ($CustomerData.coverage.PSObject.Properties.Name -contains $factorName) {
                $value = $CustomerData.coverage.$factorName
                if ($value) {
                    $multiplier = Get-Multiplier -FactorConfig $Config.coverage_options.$factorName -Value $value
                    $premium *= $multiplier
                    $multipliers[$factorName] = $multiplier
                }
            }
        }
    }
    
    # Apply discounts
    $originalPremium = $premium
    if ($CustomerData.discounts_applied) {
        foreach ($discount in $CustomerData.discounts_applied) {
            if ($Config.discounts.PSObject.Properties.Name -contains $discount) {
                $discountMultiplier = $Config.discounts.$discount.multiplier
                $premium *= $discountMultiplier
                $multipliers["${discount}_discount"] = $discountMultiplier
            }
        }
    }
    
    # Apply payment option
    if ($CustomerData.payment_option -and $Config.payment_options.PSObject.Properties.Name -contains $CustomerData.payment_option) {
        $paymentMultiplier = $Config.payment_options.($CustomerData.payment_option).multiplier
        $premium *= $paymentMultiplier
        $multipliers["payment_option"] = $paymentMultiplier
    }
    
    # Apply validation limits
    $minPremium = $Config.validation_rules.minimum_premium
    $maxPremium = $Config.validation_rules.maximum_premium
    $premium = [Math]::Max($minPremium, [Math]::Min($premium, $maxPremium))
    
    # Calculate savings
    $paymentMultiplier = if ($multipliers.ContainsKey("payment_option")) { $multipliers["payment_option"] } else { 1.0 }
    $savings = $originalPremium - ($premium / $paymentMultiplier)
    $savingsPercentage = if ($originalPremium -gt 0) { ($savings / $originalPremium) * 100 } else { 0 }
    
    return @{
        base_premium = $Config.base_premium.amount
        final_premium = [Math]::Round($premium, 2)
        currency = $Config.base_premium.currency
        payment_frequency = $CustomerData.payment_option
        multipliers = $multipliers
        savings_from_discounts = [Math]::Round($savings, 2)
        savings_percentage = [Math]::Round($savingsPercentage, 1)
    }
}

try {
    $config = Get-CalculatorConfig -ConfigPath $ConfigFile
    
    if ($ListOptions) {
        Write-Host "=== Available Factor Options ===" -ForegroundColor Green
        Write-Host "Vehicle Factors:" -ForegroundColor Yellow
        $config.vehicle_factors | ConvertTo-Json -Depth 3
        Write-Host "`nDriver Factors:" -ForegroundColor Yellow
        $config.driver_factors | ConvertTo-Json -Depth 3
        Write-Host "`nLocation Factors:" -ForegroundColor Yellow
        $config.location_factors | ConvertTo-Json -Depth 3
        return
    }
    
    if ($Example) {
        if (-not (Test-Path "example_calculation.json")) {
            throw "Example calculation file not found: example_calculation.json"
        }
        
        $exampleContent = Get-Content "example_calculation.json" -Raw | ConvertFrom-Json
        $customerData = $exampleContent.example_calculation.customer_profile
        
        $result = Calculate-Premium -Config $config -CustomerData $customerData
        
        Write-Host "=== Motor Premium Calculation ===" -ForegroundColor Green
        Write-Host "Base Premium: $($result.currency) $($result.base_premium)" -ForegroundColor White
        Write-Host "Final Premium: $($result.currency) $($result.final_premium)" -ForegroundColor Cyan
        Write-Host "Payment Frequency: $($result.payment_frequency)" -ForegroundColor White
        Write-Host "Savings from Discounts: $($result.currency) $($result.savings_from_discounts)" -ForegroundColor Green
        Write-Host "Savings Percentage: $($result.savings_percentage)%" -ForegroundColor Green
        
        Write-Host "`nMultipliers Applied:" -ForegroundColor Yellow
        foreach ($factor in $result.multipliers.GetEnumerator()) {
            Write-Host "  $($factor.Key): $($factor.Value)" -ForegroundColor White
        }
    }
    else {
        Write-Host "Motor Premium Calculator" -ForegroundColor Green
        Write-Host "Use -Example to run with sample data or -ListOptions to see available options" -ForegroundColor Yellow
        Write-Host "Example: .\calculator.ps1 -Example" -ForegroundColor Cyan
    }
}
catch {
    Write-Error "Error: $($_.Exception.Message)"
}