import AppConstants from "../constants/AppConstants";
import apiClient from './apiClient';
import {jwtDecode} from 'jwt-decode';

const authService = {
  
  /**
   * Login user with email and password
   * @param {string} email - User email
   * @param {string} password - User password
   * @returns {Promise} - User data from decoded token
   */
  login: async (email, password) => {
    try {
      const response = await apiClient.post('/api/auth/admin-login', { email, password });
      
      if (response.data.data) {
        const { accessToken, refreshToken } = response.data.data;
        localStorage.setItem(AppConstants.STORAGE_KEYS.ACCESS_TOKEN, accessToken);
        localStorage.setItem(AppConstants.STORAGE_KEYS.REFRESH_TOKEN, refreshToken);
                                
        // Extract userData from decoded jwt
        const userData = authService.getCurrentUser();
        localStorage.setItem(AppConstants.STORAGE_KEYS.USER, JSON.stringify(userData));

        return {
          ...response.data,
          userData
        };
      }
      
      return response.data;
    } catch (error) {
      if (error.response) {
        throw new Error(error.response.data.errorMessage || 'Login failed');
      }
      throw error;
    }
  },
  
  /**
   * Get current user data from token
   * @returns {Object|null} - User data from token or null if not authenticated
   */
  getCurrentUser: () => {
    const token = localStorage.getItem(AppConstants.STORAGE_KEYS.ACCESS_TOKEN);
    if (!token) return null;
    
    try {
      const decodedToken = jwtDecode(token);
      return {
        id: decodedToken[AppConstants.CLAIM_KEYS.NAME_ID],
        email: decodedToken[AppConstants.CLAIM_KEYS.EMAIL],
        role: decodedToken[AppConstants.CLAIM_KEYS.ROLE]
      };
    } catch {
      return null;
    }
  },
  
  /**
   * Logout user - clear tokens and state
   */
  logout: () => {
    localStorage.removeItem(AppConstants.STORAGE_KEYS.ACCESS_TOKEN);
    localStorage.removeItem(AppConstants.STORAGE_KEYS.REFRESH_TOKEN);
    localStorage.removeItem(AppConstants.STORAGE_KEYS.USER);
  },
  
  /**
   * Check if user is authenticated
   * @returns {boolean}
   */
  isAuthenticated: () => {
    return !!localStorage.getItem(AppConstants.STORAGE_KEYS.ACCESS_TOKEN);
  }
};

export default authService;