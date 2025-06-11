@feature
Feature: API Reset Endpoint

  Background:
    Given the test environment is set to "QA"

  Scenario: Valid user can reset the system
    When the user performs a reset with valid credentials
    Then the reset response status code should be 200

  Scenario: Invalid user cannot reset the system
    When the user performs a reset with invalid credentials
    Then the reset response status code should be 401

  Scenario: Reset without authentication is not allowed
    When the user performs a reset without authentication
    Then the reset response status code should be 401
