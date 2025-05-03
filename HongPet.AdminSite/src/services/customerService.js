import apiClient from './apiClient';

const customerService = {
  /**
   * Fetch the list of customers
   * @param {number} pageIndex - The page index for pagination
   * @param {number} pageSize - The number of items per page
   * @param {string} keyword - Search keyword (optional)
   * @returns {Promise} - List of customers
   */
  getCustomers: async (pageIndex = 1, pageSize = 10, keyword = '') => {
    try {
      const response = await apiClient.get('/admin/api/users', {
        params: { pageIndex, pageSize, keyword },
      });
      return response.data.data;
      
    } catch (error) {
      console.error('Error fetching customers:', error);
      throw error;
    }
  },

  /**
   * Fetch details of a specific customer
   * @param {string} id - Customer ID
   * @returns {Promise} - Customer details
   */
  getCustomerDetail: async (id) => {
    try {
      const response = await apiClient.get(`/api/users/${id}`);
      return response.data.data;

    } catch (error) {
      console.error(`Error fetching customer details for ID ${id}:`, error);
      throw error;
    }
  },

  /**
   * Deactivate a customer
   * @param {string} id - Customer ID
   * @returns {Promise} - Success message
   */
  deactivateCustomer: async (id) => {
    try {
      const response = await apiClient.delete(`/admin/api/users/${id}`);
      return response.data.message;
      
    } catch (error) {
      console.error(`Error deactivating customer with ID ${id}:`, error);
      throw error;
    }
  },
};

export default customerService;