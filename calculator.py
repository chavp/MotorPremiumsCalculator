#!/usr/bin/env python3
"""
Motor Premium Calculator

A simple implementation that uses the motor_premiums_calculator.json data structure
to calculate insurance premiums based on various factors.
"""

import json
import argparse
from typing import Dict, List, Any


class MotorPremiumCalculator:
    """Motor insurance premium calculator using JSON configuration."""
    
    def __init__(self, config_file: str = "motor_premiums_calculator.json"):
        """Initialize calculator with configuration data."""
        with open(config_file, 'r') as f:
            self.config = json.load(f)['motor_premiums_calculator']
        
        self.base_premium = self.config['base_premium']['amount']
        self.currency = self.config['base_premium']['currency']
    
    def calculate_premium(self, customer_data: Dict[str, Any]) -> Dict[str, Any]:
        """
        Calculate premium based on customer data.
        
        Args:
            customer_data: Dictionary containing customer profile information
            
        Returns:
            Dictionary with calculation results and breakdown
        """
        premium = self.base_premium
        multipliers = {}
        
        # Apply vehicle factors
        vehicle = customer_data.get('vehicle', {})
        premium, vehicle_multipliers = self._apply_factors(
            premium, vehicle, self.config['vehicle_factors']
        )
        multipliers.update(vehicle_multipliers)
        
        # Apply driver factors
        driver = customer_data.get('driver', {})
        premium, driver_multipliers = self._apply_factors(
            premium, driver, self.config['driver_factors']
        )
        multipliers.update(driver_multipliers)
        
        # Apply location factors
        location = customer_data.get('location', {})
        premium, location_multipliers = self._apply_factors(
            premium, location, self.config['location_factors']
        )
        multipliers.update(location_multipliers)
        
        # Apply usage factors
        usage = customer_data.get('usage', {})
        premium, usage_multipliers = self._apply_factors(
            premium, usage, self.config['usage_factors']
        )
        multipliers.update(usage_multipliers)
        
        # Apply coverage options
        coverage = customer_data.get('coverage', {})
        premium, coverage_multipliers = self._apply_factors(
            premium, coverage, self.config['coverage_options']
        )
        multipliers.update(coverage_multipliers)
        
        # Apply discounts
        discounts_applied = customer_data.get('discounts_applied', [])
        original_premium = premium
        discount_multipliers = {}
        
        for discount in discounts_applied:
            if discount in self.config['discounts']:
                discount_multiplier = self.config['discounts'][discount]['multiplier']
                premium *= discount_multiplier
                discount_multipliers[f"{discount}_discount"] = discount_multiplier
        
        # Apply payment option
        payment_option = customer_data.get('payment_option', 'monthly')
        if payment_option in self.config['payment_options']:
            payment_multiplier = self.config['payment_options'][payment_option]['multiplier']
            premium *= payment_multiplier
            multipliers['payment_option'] = payment_multiplier
        
        # Validate against limits
        min_premium = self.config['validation_rules']['minimum_premium']
        max_premium = self.config['validation_rules']['maximum_premium']
        premium = max(min_premium, min(premium, max_premium))
        
        # Calculate savings
        savings = original_premium - (premium / multipliers.get('payment_option', 1.0))
        
        return {
            'base_premium': self.base_premium,
            'final_premium': round(premium, 2),
            'currency': self.currency,
            'payment_frequency': payment_option,
            'multipliers': {**multipliers, **discount_multipliers},
            'savings_from_discounts': round(savings, 2),
            'savings_percentage': round((savings / original_premium) * 100, 1) if original_premium > 0 else 0
        }
    
    def _apply_factors(self, premium: float, data: Dict[str, Any], 
                      factors_config: Dict[str, Any]) -> tuple:
        """Apply factor multipliers to premium."""
        multipliers = {}
        
        for factor_name, factor_config in factors_config.items():
            value = data.get(factor_name)
            if not value:
                continue
                
            if 'options' in factor_config and value in factor_config['options']:
                multiplier = factor_config['options'][value]['multiplier']
                premium *= multiplier
                multipliers[factor_name] = multiplier
            elif 'ranges' in factor_config and value in factor_config['ranges']:
                multiplier = factor_config['ranges'][value]['multiplier']
                premium *= multiplier
                multipliers[factor_name] = multiplier
            elif 'enabled' in factor_config and value == 'enabled':
                multiplier = factor_config['enabled']['multiplier']
                premium *= multiplier
                multipliers[factor_name] = multiplier
            elif 'amounts' in factor_config and str(value) in factor_config['amounts']:
                multiplier = factor_config['amounts'][str(value)]['multiplier']
                premium *= multiplier
                multipliers[factor_name] = multiplier
        
        return premium, multipliers
    
    def get_factor_options(self, category: str = None) -> Dict[str, Any]:
        """Get available options for factors."""
        if category:
            return self.config.get(f"{category}_factors", {})
        
        return {
            'vehicle_factors': self.config['vehicle_factors'],
            'driver_factors': self.config['driver_factors'],
            'location_factors': self.config['location_factors'],
            'usage_factors': self.config['usage_factors'],
            'coverage_options': self.config['coverage_options'],
            'discounts': self.config['discounts'],
            'payment_options': self.config['payment_options']
        }


def main():
    """Command line interface for the calculator."""
    parser = argparse.ArgumentParser(description='Motor Premium Calculator')
    parser.add_argument('--config', default='motor_premiums_calculator.json',
                      help='Path to configuration JSON file')
    parser.add_argument('--example', action='store_true',
                      help='Run with example data')
    parser.add_argument('--list-options', action='store_true',
                      help='List available factor options')
    
    args = parser.parse_args()
    
    try:
        calculator = MotorPremiumCalculator(args.config)
        
        if args.list_options:
            options = calculator.get_factor_options()
            print(json.dumps(options, indent=2))
            return
        
        if args.example:
            # Load example calculation data
            with open('example_calculation.json', 'r') as f:
                example_data = json.load(f)
            
            customer_data = example_data['example_calculation']['customer_profile']
            result = calculator.calculate_premium(customer_data)
            
            print("=== Motor Premium Calculation ===")
            print(f"Base Premium: {calculator.currency} {result['base_premium']}")
            print(f"Final Premium: {calculator.currency} {result['final_premium']}")
            print(f"Payment Frequency: {result['payment_frequency']}")
            print(f"Savings from Discounts: {calculator.currency} {result['savings_from_discounts']}")
            print(f"Savings Percentage: {result['savings_percentage']}%")
            print("\nMultipliers Applied:")
            for factor, multiplier in result['multipliers'].items():
                print(f"  {factor}: {multiplier}")
        else:
            print("Use --example to run with sample data or --list-options to see available options")
    
    except FileNotFoundError as e:
        print(f"Error: Configuration file not found - {e}")
    except json.JSONDecodeError as e:
        print(f"Error: Invalid JSON format - {e}")
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()