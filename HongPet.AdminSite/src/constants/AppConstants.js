const AppConstants = {
    STORAGE_KEYS: {
      USER: 'user',
      ACCESS_TOKEN: 'accessToken',
      REFRESH_TOKEN: 'refreshToken',
      REMEMBER_ME: 'rememberMe',
    },

    API: {
      BASE_URL: 'https://localhost:7281',      
    },

    CLAIM_KEYS: {
      NAME_ID: "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
      EMAIL: "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress",
      ROLE: "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    },

    SUPABASE: {
      BUCKET: "hong-pet",      
    }
    
  };
  
  export default AppConstants;