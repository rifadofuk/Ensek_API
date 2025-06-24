@feature
Feature: Update an existing order

  Background:
    Given the test environment is set to "QA"
    And the system is reset to a clean state
    And the user has existing orders and inventory

  Scenario Outline: Update an existing order with a new quantity
    When the user updates the first order with quantity <NewQuantity>
    Then the order should be updated successfully with the quantity <NewQuantity>

    Examples:
      | NewQuantity |
      | 5           |
      | 10          |
      | 20          |

# Scenario: Update multiple orders with new quantities
#  When the user updates the following orders:
 #   | OrderId | NewQuantity | ProductType |
#    | 101     | 5           | Gas         |
 #   | 102     | 10          | Electricity |
 # Then all orders should be updated successfully