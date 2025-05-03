import apiClient from './apiClient';

const categoryService = {
  /**
   * Fetch the list of categories
   * @param {number} pageIndex - The page index for pagination
   * @param {number} pageSize - The number of items per page
   * @param {string} keyword - Search keyword (optional)
   * @returns {Promise} - List of categories
   */
  getCategories: async (pageIndex = 1, pageSize = 10, keyword = '') => {
    try {
      const params = { pageIndex, pageSize, keyword };
      const response = await apiClient.get('/api/categories', { params });
      return response.data.data;
    } catch (error) {
      console.error('Error fetching categories:', error);
      throw error;
    }
  },

  /**
   * Fetch all categories without pagination
   * @returns {Promise} - List of all categories
   */
  getAllCategories: async () => {
    try {
      const response = await apiClient.get('/api/categories/all');
      return response.data.data;
    } catch (error) {
      console.error('Error fetching all categories:', error);
      throw error;
    }
  },

  /**
   * Create a new category
   * @param {object} categoryData - Category data
   * @returns {Promise} - Created category ID
   */
  createCategory: async (categoryData) => {
    try {
      const response = await apiClient.post('/api/categories', categoryData);
      return response.data.data;
    } catch (error) {
      console.error('Error creating category:', error);
      throw error;
    }
  },

  /**
   * Get a specific category by ID
   * @param {string} id - Category ID
   * @returns {Promise} - Category details
   */
  getCategoryById: async (id) => {
    try {
      const response = await apiClient.get(`/api/categories/${id}`);
      return response.data.data;
    } catch (error) {
      console.error(`Error fetching category with ID ${id}:`, error);
      throw error;
    }
  },

  /**
   * Update a category
   * @param {string} id - Category ID
   * @param {object} categoryData - Updated category data
   * @returns {Promise} - Updated category
   */
  updateCategory: async (id, categoryData) => {
    try {
      const response = await apiClient.put(`/api/categories/${id}`, categoryData);
      return response.data.data;
    } catch (error) {
      console.error(`Error updating category with ID ${id}:`, error);
      throw error;
    }
  },

  /**
   * Delete a category
   * @param {string} id - Category ID
   * @returns {Promise} - Success message
   */
  deleteCategory: async (id) => {
    try {
      const response = await apiClient.delete(`/api/categories/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error deleting category with ID ${id}:`, error);
      throw error;
    }
  }
};

export default categoryService;