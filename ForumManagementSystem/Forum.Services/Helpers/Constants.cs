namespace Business.Services.Helpers
{
    public static class Constants
    {
        //Constants for user
        public const string ModifyUserErrorMessage = "Only owner or admin can modify or delete a user.";
        public const string ModifyUsernameErrorMessage = "Username change is not allowed.";

        //Constants for category
        public const string ModifyCategoryErrorMessage = "Only an admin can modify a category.";
        public const string CategoryExistingErrorMessage = "Category with this name already exists.";

        //Constants for post
        public const string ModifyPostErrorMessage = "Only an admin or post creator can modify or delete a post.";
        public const string ModifyPostErrorMessageIfUserIsBlocked = "Blocked user cannot create a post.";

        //Constants for comment
        public const string ModifyCommentErrorMessage = "Only an admin or unblocked user can create or modify a comment.";
    }
}
