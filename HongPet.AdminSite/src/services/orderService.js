import apiClient from './apiClient';

const orderService = {
  /**
   * Fetch the list of orders
   * @param {number} pageIndex - The page index for pagination
   * @param {number} pageSize - The number of items per page
   * @param {string} keyword - Search keyword (optional)
   * @returns {Promise} - List of orders
   */
  getOrders: async (pageIndex = 1, pageSize = 10, keyword = '') => {
    try {
      const response = await apiClient.get('/admin/api/orders', {
        params: { pageIndex, pageSize, keyword },
      });
      return response.data.data;
      
    } catch (error) {
      console.error('Error fetching orders:', error);
      throw error;
    }
  },

  /**
   * Fetch details of a specific order
   * @param {string} id - Order ID
   * @returns {Promise} - Order details
   */
  getOrderDetail: async (id) => {
    try {
      const response = await apiClient.get(`/api/orders/${id}`);
      return response.data.data;

    } catch (error) {
      console.error(`Error fetching order details for ID ${id}:`, error);
      throw error;
    }
  },
};

export default orderService;