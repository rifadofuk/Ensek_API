@feature
Feature: Buy Energy Products
  As an authenticated or unauthenticated user
  I want to be able to buy energy products
  So that I can verify system behavior under different conditions

  Background:
    Given the test environment is set to "QA"
    Given the system is reset to a clean state

  Scenario: Authenticated user buys a product
    When the user buys product with ID "1" and quantity "2"
    Then the response code should be 200

  Scenario: Unauthorized user cannot buy a product
    Given the user's credentials are overridden with "special_user" and "special_password"
    When the user buys product with ID "1" and quantity "2"
    Then the response code should be 401

  Scenario: Unauthenticated user buys a product
    When an unauthenticated user buys product with ID "1" and quantity "2"
    Then the response code should be 200

  Scenario: Buying product with invalid IDs results in bad request
    When the user buys product with ID "1124422324242424" and quantity "242424242424"
    Then the response code should be 400

  Scenario: Buy each available fuel and validate response
    When the user fetches available fuels and buys each with quantity "1"
    Then all purchase responses should be valid
