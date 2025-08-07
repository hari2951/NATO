namespace TransactionApp.Application.Utilities
{
    public static class LogMessages
    {
        public const string CreatingUser = "Creating user: {UserName}";
        public const string UserCreated = "User created: {UserId}";

        public const string FetchingUser = "Fetching user with ID: {UserId}";
        public const string UserFound = "User found: {UserId}";
        public const string UserNotFound = "User not found: {UserId}";

        public const string UpdatingUser = "Updating user with ID: {UserId}";
        public const string UserUpdated = "User updated: {UserId}";

        public const string DeletingUser = "Deleting user with ID: {UserId}";
        public const string UserDeleted = "User deleted: {UserId}";

        public const string FetchingAllUsers = "Fetching all users";

        public const string CreatingTransaction = "Creating new transaction for user {UserId}";
        public const string TransactionCreated = "Transaction created with ID {TransactionId}";

        public const string FetchingTransaction = "Fetching transaction with ID {TransactionId}";
        public const string TransactionNotFound = "Transaction with ID {TransactionId} not found";
    }
}