using System.Text.RegularExpressions;

namespace TransactionApp.Application.Utilities
{
    public static class LogMessages
    {
        public const string CreatingUser = "Creating user: {UserName}";
        public const string FailedCreatingUser = "Failed to create user: {UserName}";
        public const string UserCreated = "User created: {UserId}";

        public const string FetchingUser = "Fetching user with ID: {UserId}";
        public const string UserFound = "User found: {UserId}";
        public const string UserNotFound = "User not found: {UserId}";

        public const string UpdatingUser = "Updating user with ID: {UserId}";
        public const string FailedUpdatingUser = "Failed to update user with ID: {UserId}";
        public const string UserUpdated = "User updated: {UserId}";

        public const string DeletingUser = "Deleting user with ID: {UserId}";
        public const string FailedDeletingUser = "Failed to delete user with ID: {UserId}";
        public const string UserDeleted = "User deleted: {UserId}";

        public const string FetchingAllUsers = "Fetching all users";
        public const string FailedFetchingAllUsers = "Failed to fetch all users";
        public const string FailedFetchingUser = "Failed to fetch user with ID: {UserId}";

        public const string CreatingTransaction = "Creating new transaction for user {UserId}";
        public const string TransactionCreated = "Transaction created with ID {TransactionId}";

        public const string FetchingTransaction = "Fetching transaction with ID {TransactionId}";
        public const string FetchingAllTransactions = "Fetching all transactions"; 
        public const string FetchingAllTransactionsDetails = "Fetching all transactions (page {PageNumber}, size {PageSize})";
        public const string TransactionNotFound = "Transaction with ID {TransactionId} not found";

        public const string FetchingTransactionUserWarning = "UserId is required.";
        public const string FetchingTransactionsSummary = "Fetching transactions for user: {UserId}, transaction type: {Type}";
        public const string FetchingTransactionsSummaryFromDb = "Fetching transactions from the database";
        public const string FetchingTransactionSummaryFromCache = "Fetching transactions from the cache for the key: {CacheKey}";
        public const string FetchingTransactionSummaryError = "Failed to compute transaction summary";

        public const string TransactionCacheCleared = "Cleared cache with prefix: {CachePrefix}";

        public const string UserNotPresenForException = "User with ID {0} does not exist.";
        public const string UserNotPresenForLog = "User with ID {UserId} does not exist.";
        public const string FailedCreatingTransaction = "An error occurred while creating a transaction for user {UserId}";
        public const string FailedFetchingTransaction = "An error occurred while retrieving transaction with ID {TransactionId}";
        public const string FailedFetchingTransactions = "An error occurred while retrieving all transactions.";
    }
}