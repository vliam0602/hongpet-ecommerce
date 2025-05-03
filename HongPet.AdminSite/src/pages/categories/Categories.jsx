import { useState, useEffect } from 'react'
import { Plus, Edit, Trash2, X } from 'lucide-react'
import categoryService from '../../services/categoryService'

function Categories() {
  const [categories, setCategories] = useState([])
  const [allCategories, setAllCategories] = useState([]) // Lưu tất cả categories cho dropdown
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [showModal, setShowModal] = useState(false)
  const [editingCategory, setEditingCategory] = useState(null)
  const [categoryName, setCategoryName] = useState('')
  const [parentCategoryId, setParentCategoryId] = useState('')
  
  const [pagination, setPagination] = useState({
    pageIndex: 1,
    pageSize: 10,
    totalItems: 0,
    totalPages: 0
  })
  
  useEffect(() => {
    fetchCategories(pagination.pageIndex, pagination.pageSize);
    // Lấy tất cả categories cho dropdown
    fetchAllCategories();
  }, [pagination.pageIndex, pagination.pageSize])
  
  const fetchCategories = async (pageIndex = 1, pageSize = 10) => {
    try {
      setLoading(true);
      const data = await categoryService.getCategories(pageIndex, pageSize);
      
      setCategories(data.items || []);
      
      setPagination({
        pageIndex: data.currentPage,
        pageSize: data.pageSize,
        totalItems: data.totalCount,
        totalPages: data.totalPages
      });
      
      setError(null);
    } catch (err) {
      console.error('Error fetching categories:', err);
      setError('Failed to load categories. Please try again later.');
    } finally {
      setLoading(false);
    }
  };
  
  const fetchAllCategories = async () => {
    try {
      const data = await categoryService.getAllCategories();
      setAllCategories(data || []);
    } catch (err) {
      console.error('Error fetching all categories:', err);
      // Không set error vì đây là chức năng phụ, chính là danh sách phân trang
    }
  };
  
  const handleAddCategory = () => {
    setEditingCategory(null)
    setCategoryName('')
    setParentCategoryId('')
    setShowModal(true)
  }
  
  const handleEditCategory = (category) => {
    setEditingCategory(category)
    setCategoryName(category.name)
    setParentCategoryId(category.parentCategoryId || '')
    setShowModal(true)
  }
  
  const handleDeleteCategory = async (categoryId) => {    
    const categoryToDelete = categories.find(cat => cat.id === categoryId);
    if (!categoryToDelete) return;
    
    const categoryName = categoryToDelete.name;
    
    if (window.confirm('Are you sure you want to delete this category?')) {
      try {
        setLoading(true);
        // call api to delete category (hard delete)
        await categoryService.deleteCategory(categoryId);
                
        alert(`Delete ${categoryName} successfully`);
        
        // deleted successfully, refresh the data
        fetchCategories(pagination.pageIndex, pagination.pageSize);
        fetchAllCategories(); // update the parent categories for drop down
      } catch (err) {
        console.error('Error deleting category:', err);
        alert('Failed to delete category. Please try again.');
      } finally {
        setLoading(false);
      }
    }
  }
  
  const handleSubmit = async (e) => {
    e.preventDefault()
    
    if (!categoryName.trim()) {
      alert('Please enter a category name')
      return
    }
    
    try {
      setLoading(true);
      const categoryData = {
        name: categoryName,
        parentCategoryId: parentCategoryId || null
      };
      
      if (editingCategory) {
        // Update existing category
        await categoryService.updateCategory(editingCategory.id, categoryData);
      } else {
        // Add new category
        await categoryService.createCategory(categoryData);
      }
      
      // Refresh categories after update
      fetchCategories(pagination.pageIndex, pagination.pageSize);
      fetchAllCategories(); // Cập nhật lại tất cả danh mục cho dropdown
      setShowModal(false);
    } catch (err) {
      console.error('Error saving category:', err);
      alert('Failed to save category. Please try again.');
    } finally {
      setLoading(false);
    }
  }
  
  if (loading && categories.length === 0) {
    return <div className="flex justify-center items-center h-full">Loading categories...</div>
  }
  
  if (error) {
    return <div className="flex justify-center items-center h-full text-red-500">{error}</div>
  }
  
  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Categories</h1>
        <button 
          className="btn btn-primary flex items-center gap-2"
          onClick={handleAddCategory}
        >
          <Plus size={18} />
          Add Category
        </button>
      </div>
      
      <div className="card">
        <div className="table-container">
          <table className="table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Parent Category</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {categories.length > 0 ? (
                categories.map((category) => (
                  <tr key={category.id}>
                    <td className="font-medium">{category.name}</td>
                    <td>
                      {category.parentCategoryId 
                        ? allCategories.find(c => c.id === category.parentCategoryId)?.name || '-' 
                        : '-'}
                    </td>
                    <td>
                      <div className="flex items-center gap-2">
                        <button 
                          className="p-1 text-amber-600 hover:text-amber-800"
                          onClick={() => handleEditCategory(category)}
                        >
                          <Edit size={18} />
                        </button>
                        <button 
                          className="p-1 text-red-600 hover:text-red-800"
                          onClick={() => handleDeleteCategory(category.id)}
                        >
                          <Trash2 size={18} />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="3" className="text-center py-4">No categories found</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
        
        {/* Pagination */}
        {pagination.totalPages > 1 && (
          <div className="flex justify-between items-center mt-4">
              <div>
                Showing {(pagination.pageIndex - 1) * pagination.pageSize + 1} to {Math.min(pagination.pageIndex * pagination.pageSize, pagination.totalItems)} of {pagination.totalItems} entries
              </div>
              <div className="flex gap-2">
                {pagination.pageIndex > 1 && (
                  <button 
                    className="btn btn-dark"
                    onClick={() => setPagination(prev => ({ ...prev, pageIndex: prev.pageIndex - 1 }))}
                  >
                    Previous
                  </button>
                )}
                {pagination.pageIndex < pagination.totalPages && (
                  <button
                  className="btn btn-dark"
                  onClick={() => setPagination(prev => ({ ...prev, pageIndex: prev.pageIndex + 1 }))}
                  >
                    Next
                  </button>
                )}
              </div>
          </div>
        )}
      </div>
      
      {/* Add/Edit Category Modal */}
      {showModal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-white rounded-lg p-6 w-full max-w-md">
            {/* Modal content */}
            <div className="flex justify-between items-center mb-4">
              <h2 className="text-xl font-semibold">
                {editingCategory ? 'Edit Category' : 'Add Category'}
              </h2>
              <button 
                className="text-gray-500 hover:text-gray-700"
                onClick={() => setShowModal(false)}
              >
                <X size={20} />
              </button>
            </div>
            
            <form onSubmit={handleSubmit}>
              {/* form fields */}
              <div className="mb-4">
                <label htmlFor="categoryName" className="block mb-1 font-medium">
                  Category Name <span className="text-red-500">*</span>
                </label>
                <input
                  id="categoryName"
                  type="text"
                  className="input-field"
                  placeholder="Enter category name"
                  value={categoryName}
                  onChange={(e) => setCategoryName(e.target.value)}
                  required
                />
              </div>
              
              <div className="mb-6">
                <label htmlFor="parentCategory" className="block mb-1 font-medium">
                  Parent Category
                </label>
                <select
                  id="parentCategory"
                  className="input-field"
                  value={parentCategoryId}
                  onChange={(e) => setParentCategoryId(e.target.value)}
                >
                  <option value="">None (Top Level)</option>
                  {/* Sử dụng tất cả danh mục từ API all categories */}
                  {allCategories
                    .filter(cat => cat.id !== (editingCategory?.id || ''))
                    .map(category => (
                      <option key={category.id} value={category.id}>
                        {category.name}
                      </option>
                    ))
                  }
                </select>
              </div>
              
              <div className="flex justify-end gap-2">
                <button 
                  type="button" 
                  className="btn bg-gray-200 text-gray-800 hover:bg-gray-300"
                  onClick={() => setShowModal(false)}
                >
                  Cancel
                </button>
                <button type="submit" className="btn btn-primary">
                  {editingCategory ? 'Update' : 'Add'} Category
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
      
      {loading && categories.length > 0 && 
        <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-40">
          <div className="bg-white rounded-lg p-4">
            <p className="text-center">Processing...</p>
          </div>
        </div>
      }
    </div>
  )
}

export default Categories