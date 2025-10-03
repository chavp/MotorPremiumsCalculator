# Motor Premiums Calculator

A comprehensive JSON data structure and implementation for calculating motor insurance premiums based on multiple risk factors.

## Overview

This motor premiums calculator is designed to assess insurance risk and calculate premiums based on:

- **Vehicle Factors**: Type, engine capacity, age, value, safety rating
- **Driver Factors**: Age, experience, driving record, demographics
- **Location Factors**: Area type, crime rate, weather conditions
- **Usage Factors**: Annual mileage, usage type, parking situation
- **Coverage Options**: Liability, comprehensive, collision, deductible levels
- **Discounts**: Various available discounts to reduce premiums
- **Payment Options**: Different payment frequencies with associated costs

## Files

### Core Data Structure
- **`motor_premiums_calculator.json`** - Complete configuration with all factors, multipliers, and rules
- **`example_calculation.json`** - Example calculation showing how the system works
- **`calculator.py`** - Python implementation demonstrating usage
- **`README.md`** - This documentation file

## JSON Structure

### Base Configuration
```json
{
  "motor_premiums_calculator": {
    "version": "1.0",
    "base_premium": {
      "amount": 500.00,
      "currency": "USD"
    }
  }
}
```

### Factor Categories

#### Vehicle Factors
- **Vehicle Type**: Private car, commercial vehicle, motorcycle, truck, bus
- **Engine Capacity**: Ranges from under 1000cc to above 3000cc
- **Vehicle Age**: New to over 10 years old
- **Vehicle Value**: Under $10k to luxury vehicles above $100k
- **Safety Rating**: 5-star to unrated vehicles

#### Driver Factors
- **Age**: Under 21 to above 65 years
- **Experience**: 0-1 years to above 20 years of driving
- **Driving Record**: Clean record to multiple violations
- **Gender**: Statistical risk adjustments
- **Marital Status**: Married, single, divorced, widowed

#### Location Factors
- **Area Type**: Urban, suburban, rural
- **Crime Rate**: Low, medium, high
- **Weather Conditions**: Mild, seasonal harsh, extreme

#### Usage Factors
- **Annual Mileage**: Under 5k to above 25k miles per year
- **Usage Type**: Personal, commuting, business, commercial, rideshare
- **Parking**: Private garage, driveway, street, public lot

#### Coverage Options
- **Liability Coverage**: Minimum required to maximum coverage
- **Comprehensive Coverage**: Optional theft/damage protection
- **Collision Coverage**: Optional accident damage protection
- **Deductible**: $250 to $5,000 options

#### Discounts Available
- Multi-policy bundling
- Safe driver record
- Defensive driving course
- Anti-theft systems
- Good student (for young drivers)
- Senior citizen
- Low annual mileage
- Automatic payment setup
- Paperless billing

## Calculation Formula

The premium calculation follows this formula:

```
final_premium = base_premium × 
                vehicle_factors × 
                driver_factors × 
                location_factors × 
                usage_factors × 
                coverage_options × 
                discounts × 
                payment_option
```

### Validation Rules
- **Minimum Premium**: $200.00
- **Maximum Premium**: $10,000.00
- **Required Fields**: Vehicle type, engine capacity, vehicle age, driver age, experience, area type, annual mileage, usage type, liability coverage

## Usage Examples

### Using the Python Calculator

```bash
# Run with example data
python calculator.py --example

# List all available options
python calculator.py --list-options

# Use custom configuration file
python calculator.py --config custom_config.json --example
```

### Example Output
```
=== Motor Premium Calculation ===
Base Premium: USD 500.0
Final Premium: USD 910.33
Payment Frequency: semi_annual
Savings from Discounts: USD 449.3
Savings Percentage: 33.1%

Multipliers Applied:
  vehicle_type: 1.0
  engine_capacity: 1.3
  vehicle_age: 0.9
  ...
```

### Sample Customer Profile
```json
{
  "vehicle": {
    "type": "private_car",
    "engine_capacity": "1500_2000cc",
    "age": "3_5_years",
    "value": "25000_50000",
    "safety_rating": "4_star"
  },
  "driver": {
    "age": "31_50",
    "experience": "11_20_years",
    "driving_record": "clean_record",
    "gender": "female",
    "marital_status": "married"
  },
  "discounts_applied": [
    "multi_policy",
    "safe_driver",
    "anti_theft"
  ],
  "payment_option": "semi_annual"
}
```

## Integration Guidelines

### API Integration
The JSON structure is designed for easy integration with:
- REST APIs
- Database storage
- Web applications
- Mobile apps
- Third-party insurance systems

### Customization
To customize for different markets or insurance products:

1. **Adjust Multipliers**: Modify risk multipliers based on actuarial data
2. **Add Factors**: Include additional risk factors in respective categories
3. **Currency**: Change base currency and amounts
4. **Validation Rules**: Adjust minimum/maximum premium limits
5. **Regional Factors**: Add location-specific considerations

### Data Validation
Ensure all customer inputs match the defined options and ranges in the JSON structure. The calculator includes validation against:
- Required fields
- Valid option values
- Premium limits
- Logical combinations

## Technical Notes

### Performance Considerations
- JSON structure is optimized for fast lookups
- All calculations use simple multiplication
- Validation rules prevent extreme values
- Caching can be implemented for frequently accessed factors

### Security Considerations
- Validate all input data against the schema
- Implement rate limiting for calculation requests
- Log calculation requests for audit purposes
- Ensure sensitive customer data is properly encrypted

### Maintenance
- Regularly review and update multipliers based on claims data
- Add new discount types as they become available
- Update coverage options to match regulatory changes
- Monitor calculation accuracy and adjust base premiums as needed

## License and Usage

This data structure and implementation are provided as examples for motor insurance premium calculation. Adapt and modify according to your specific business requirements, regulatory compliance needs, and actuarial data.

For production use, ensure:
- Compliance with local insurance regulations
- Validation against actuarial data
- Regular updates to risk factors
- Proper testing and quality assurance