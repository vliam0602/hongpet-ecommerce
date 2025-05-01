import { useState, useEffect } from 'react'
import { Plus, Edit, Trash2, X } from 'lucide-react'

// Mock data for categories
const mockCategories = [
  { id: '1', name: 'Pet Food', parentCategoryId: null, subCategories: ['2', '3'] },
  { id: '2', name: 'Dog Food', parentCategoryId: '1', subCategories: [] },
  { id: '3', name: 'Cat Food', parentCategoryId: '1', subCategories: [] },
  { id: '4', name: 'Toys', parentCategoryId: null, subCategories: ['5', '6'] },
  { id: '5', name: 'Dog Toys', parentCategoryId: '4', subCategories: [] },
  { id: '6', name: 'Cat Toys', parentCategoryId: '4', subCategories: [] },
  { id: '7', name: 'Accessories', parentCategoryId: null, subCategories: [] },
  { id: '8', name: 'Grooming', parentCategoryId: null, subCategories: [] },
  { id: '9', name: 'Health & Wellness', parentCategoryId: null, subCategories: [] },
]

function Categories() {
  const [categories, setCategories] = useState([])
  const [loading, setLoading] = useState(true)
  const [showModal, setShowModal] = useState(false)
  const [editingCategory, setEditingCategory] = useState(null)
  const [categoryName, setCategoryName] = useState('')
  const [parentCategoryId, setParentCategoryId] = useState('')
  
  useEffect(() => {
    // Simulate API call
    setTimeout(() => {
      setCategories(mockCategories)
      setLoading(false)
    }, 500)
  }, [])
  
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
  
  const handleDeleteCategory = (categoryId) => {
    if (window.confirm('Are you sure you want to delete this category?')) {
      // In a real app, you would call an API to delete the category
      setCategories(categories.filter(category => category.id !== categoryId))
    }
  }
  
  const handleSubmit = (e) => {
    e.preventDefault()
    
    if (!categoryName.trim()) {
      alert('Please enter a category name')
      return
    }
    
    if (editingCategory) {
      //  {
      alert('Please enter a category name')
      return
    }
    
    if (editingCategory) {
      // Update existing category
      setCategories(categories.map(category => 
        category.id === editingCategory.id 
          ? { ...category, name: categoryName, parentCategoryId: parentCategoryId || null }
          : category
      ))
    } else {
      // Add new category
      const newCategory = {
        id: Date.now().toString(),
        name: categoryName,
        parentCategoryId: parentCategoryId || null,
        subCategories: []
      }
      
      setCategories([...categories, newCategory])
      
      // If this category has a parent, update the parent's subCategories
      if (parentCategoryId) {
        setCategories(categories.map(category => 
          category.id === parentCategoryId
            ? { ...category, subCategories: [...category.subCategories, newCategory.id] }
            : category
        ))
      }
    }
    
    setShowModal(false)
  }
  
  // Get only parent categories (for dropdown)
  const parentCategories = categories.filter(category => !category.parentCategoryId)
  
  // Group categories by parent for display
  const groupedCategories = {}
  parentCategories.forEach(parent => {
    groupedCategories[parent.id] = {
      parent,
      children: categories.filter(cat => cat.parentCategoryId === parent.id)
    }
  })
  
  if (loading) {
    return <div className="flex justify-center items-center h-full">Loading categories...</div>
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
                        ? categories.find(c => c.id === category.parentCategoryId)?.name 
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
      </div>
      
      {/* Category Tree View */}
      <div className="card">
        <h2 className="text-lg font-semibold mb-4">Category Hierarchy</h2>
        
        {Object.values(groupedCategories).length > 0 ? (
          <div className="space-y-4">
            {Object.values(groupedCategories).map(({ parent, children }) => (
              <div key={parent.id} className="border border-gray-200 rounded-lg p-4">
                <h3 className="font-medium text-lg">{parent.name}</h3>
                {children.length > 0 ? (
                  <div className="mt-2 pl-4 border-l-2 border-gray-200">
                    {children.map(child => (
                      <div key={child.id} className="py-1">
                        {child.name}
                      </div>
                    ))}
                  </div>
                ) : (
                  <p className="text-sm text-gray-500 mt-2">No subcategories</p>
                )}
              </div>
            ))}
          </div>
        ) : (
          <p className="text-gray-500">No categories found</p>
        )}
      </div>
      
      {/* Add/Edit Category Modal */}
      {showModal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-white rounded-lg p-6 w-full max-w-md">
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
                  {parentCategories
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
    </div>
  )
}

export default Categories