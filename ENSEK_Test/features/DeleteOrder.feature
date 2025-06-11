@feature
Feature: Delete Orders
  As an authenticated user
  I want to delete a valid energy order
  So that I can manage energy transactions effectively

  Background:
    Given the test environment is set to "QA"
    And the system is reset to a clean state

  Scenario: Authenticated user deletes a valid order
    When the user retrieves the list of orders
    And the user deletes the first order
    Then the order deletion should return status code 200
