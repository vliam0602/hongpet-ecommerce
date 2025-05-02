import axios from 'axios';
import AppConstants from "../constants/AppConstants";

const BASE_URL = AppConstants.API.BASE_URL;

const apiClient = axios.create({
  baseURL: BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  },
  timeout: 10000
});

// Request interceptor for adding auth token
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem(AppConstants.STORAGE_KEYS.ACCESS_TOKEN);
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor for handling common errors
apiClient.interceptors.response.use(
  (response) => {
    return response;
  },
  async (error) => {
    const originalRequest = error.config;
    
    // Handle 401 errors (unauthorized) - token expired
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      
      try {
        // Try to refresh the token
        const refreshToken = localStorage.getItem(AppConstants.STORAGE_KEYS.REFRESH_TOKEN);
        if (refreshToken) {
          const response = await axios.get(`${BASE_URL}/auth/refresh-token`, {
            headers: {
              'Authorization': `Bearer ${refreshToken}`
            }
          });
          
          if (response.data.data) {
            const { accessToken, refreshToken: newRefreshToken } = response.data.data;
            localStorage.setItem(AppConstants.STORAGE_KEYS.ACCESS_TOKEN, accessToken);
            localStorage.setItem(AppConstants.STORAGE_KEYS.REFRESH_TOKEN, newRefreshToken);
            
            // Retry the original request with new token
            originalRequest.headers['Authorization'] = `Bearer ${accessToken}`;
            return axios(originalRequest);
          }
        }
      } catch (refreshError) {
        // Handle refresh token failure - logout user
        localStorage.removeItem(AppConstants.STORAGE_KEYS.ACCESS_TOKEN);
        localStorage.removeItem(AppConstants.STORAGE_KEYS.REFRESH_TOKEN);
        localStorage.removeItem(AppConstants.STORAGE_KEYS.USER);
        window.location.href = '/login';
      }
    }
    
    return Promise.reject(error);
  }
);

export default apiClient;