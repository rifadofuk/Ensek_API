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
