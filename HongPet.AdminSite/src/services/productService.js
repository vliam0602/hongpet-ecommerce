import apiClient from './apiClient';

const productService = {
  /**
   * Fetch the list of products
   * @param {number} pageIndex - The page index for pagination
   * @param {number} pageSize - The number of items per page
   * @param {string} keyword - Search keyword (optional)
   * @returns {Promise} - List of products
   */
  getProducts: async (pageIndex = 1, pageSize = 10, keyword = '') => {
    try {
      const params = { pageIndex, pageSize, keyword };      
      
      const response = await apiClient.get('/api/products', { params });
      return response.data.data;
      
    } catch (error) {
      console.error('Error fetching products:', error);
      throw error;
    }
  },

  /**
   * Fetch details of a specific product
   * @param {string} id - Product ID
   * @returns {Promise} - Product details
   */
  getProductDetail: async (id) => {
    try {
      const response = await apiClient.get(`/api/products/${id}`);
      return response.data.data;

    } catch (error) {
      console.error(`Error fetching product details for ID ${id}:`, error);
      throw error;
    }
  },

  /**
   * Add a new product
   * @param {object} productData - Product data
   * @returns {Promise} - Created product details
   */
  addProduct: async (productData) => {
    try {
      const response = await apiClient.post('/admin/api/products', productData);
      return response.data.data;
    } catch (error) {
      console.error('Error adding product:', error);
      throw error;
    }
  },

  /**
   * Update a product
   * @param {string} id - Product ID
   * @param {object} productData - Updated product data
   * @returns {Promise} - Updated product details
   */
  updateProduct: async (id, productData) => {
    try {
      const response = await apiClient.put(`/admin/api/products/${id}`, productData);
      return response.data.data;
    } catch (error) {
      console.error(`Error updating product with ID ${id}:`, error);
      throw error;
    }
  },

  /**
   * Delete a product
   * @param {string} id - Product ID
   * @returns {Promise} - Success message
   */
  deleteProduct: async (id) => {
    try {
      const response = await apiClient.delete(`/admin/api/products/${id}`);
      return response.data;
      
    } catch (error) {
      console.error(`Error deleting product with ID ${id}:`, error);
      throw error;
    }
  },

  /**
   * Fetch all available product attributes
   * @returns {Promise} - List of all attributes
   */
  getAllAttributes: async () => {
    try {
      const response = await apiClient.get('/api/products/attributes');
      return response.data.data;
    } catch (error) {
      console.error('Error fetching attributes:', error);
      throw error;
    }
  }
  
};

export default productService;