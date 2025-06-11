@feature
Feature: Get and validate orders

  Background:
    Given the test environment is set to "QA"
    And the system is reset to a clean state

  Scenario: Buy available fuels and validate placed orders
    When the user buys all available fuels
    Then the placed orders should be found in the order list

  Scenario: Count orders created before today
    When the user retrieves the list of All orders
    Then the number of orders created before today should be shown
