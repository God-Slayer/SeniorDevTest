namespace ShoppingListApi.Constants
{
    public static class FBConstants
    {
        public static readonly string ValidateEndpoint = "https://graph.facebook.com/debug_token?input_token=";

        public static readonly string UserEndpoint = "https://graph.facebook.com/me?fields=first_name,last_name,email,id&access_token=";
    }
}