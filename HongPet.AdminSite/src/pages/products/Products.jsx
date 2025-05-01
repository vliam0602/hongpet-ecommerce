import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { Plus, Search, Edit, Trash2, Eye } from 'lucide-react'

// Mock data for products
const mockProducts = [
  { 
    id: '1', 
    name: 'Premium Dog Food', 
    description: 'High-quality dog food for all breeds',
    price: 29.99,
    thumbnailUrl: 'https://placehold.co/100x100',
    isActive: true,
    categories: ['Pet Food', 'Dogs'],
    variants: 5,
    createdDate: '2025-04-15'
  },
  { 
    id: '2', 
    name: 'Cat Toy Set', 
    description: 'Interactive toys for cats',
    price: 24.99,
    thumbnailUrl: 'https://placehold.co/100x100',
    isActive: true,
    categories: ['Toys', 'Cats'],
    variants: 3,
    createdDate: '2025-04-10'
  },
  { 
    id: '3', 
    name: 'Bird Cage', 
    description: 'Spacious cage for small birds',
    price: 49.99,
    thumbnailUrl: 'https://placehold.co/100x100',
    isActive: true,
    categories: ['Cages', 'Birds'],
    variants: 2,
    createdDate: '2025-04-05'
  },
  { 
    id: '4', 
    name: 'Fish Tank Filter', 
    description: 'Advanced filtration system for aquariums',
    price: 34.99,
    thumbnailUrl: 'https://placehold.co/100x100',
    isActive: false,
    categories: ['Aquarium', 'Fish'],
    variants: 4,
    createdDate: '2025-03-28'
  },
  { 
    id: '5', 
    name: 'Hamster Wheel', 
    description: 'Exercise wheel for small pets',
    price: 15.99,
    thumbnailUrl: 'https://placehold.co/100x100',
    isActive: true,
    categories: ['Small Pets', 'Accessories'],
    variants: 1,
    createdDate: '2025-03-20'
  },
]

function Products() {
  const [products, setProducts] = useState([])
  const [loading, setLoading] = useState(true)
  const [searchTerm, setSearchTerm] = useState('')
  const [filterActive, setFilterActive] = useState('all')

  useEffect(() => {
    // Simulate API call
    setTimeout(() => {
      setProducts(mockProducts)
      setLoading(false)
    }, 500)
  }, [])

  const handleDelete = (id) => {
    if (window.confirm('Are you sure you want to delete this product?')) {
      // In a real app, you would call an API to delete the product
      setProducts(products.filter(product => product.id !== id))
    }
  }

  const filteredProducts = products.filter(product => {
    const matchesSearch = product.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                          product.description.toLowerCase().includes(searchTerm.toLowerCase())
    
    if (filterActive === 'all') return matchesSearch
    if (filterActive === 'active') return matchesSearch && product.isActive
    if (filterActive === 'inactive') return matchesSearch && !product.isActive
    
    return matchesSearch
  })

  if (loading) {
    return <div className="flex justify-center items-center h-full">Loading products...</div>
  }

  return (
    <div className="space-y-6">
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <h1 className="text-2xl font-bold">Products</h1>
        <Link to="/products/add" className="btn btn-primary flex items-center gap-2">
          <Plus size={18} />
          Add New Product
        </Link>
      </div>

      <div className="card">
        <div className="flex flex-col md:flex-row gap-4 mb-6">
          <div className="relative flex-1">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" size={18} />
            <input
              type="text"
              placeholder="Search products..."
              className="input-field pl-10"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
          <div className="flex gap-2">
            <button 
              className={`btn ${filterActive === 'all' ? 'btn-primary' : 'btn-dark'}`}
              onClick={() => setFilterActive('all')}
            >
              All
            </button>
            <button 
              className={`btn ${filterActive === 'active' ? 'btn-primary' : 'btn-dark'}`}
              onClick={() => setFilterActive('active')}
            >
              Active
            </button>
            <button 
              className={`btn ${filterActive === 'inactive' ? 'btn-primary' : 'btn-dark'}`}
              onClick={() => setFilterActive('inactive')}
            >
              Inactive
            </button>
          </div>
        </div>

        <div className="table-container">
          <table className="table">
            <thead>
              <tr>
                <th>Image</th>
                <th>Name</th>
                <th className="hidden md:table-cell">Price</th>
                <th className="hidden md:table-cell">Categories</th>
                <th className="hidden md:table-cell">Variants</th>
                <th className="hidden md:table-cell">Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {filteredProducts.length > 0 ? (
                filteredProducts.map((product) => (
                  <tr key={product.id}>
                    <td>
                      <img 
                        src={product.thumbnailUrl || "/placeholder.svg"} 
                        alt={product.name} 
                        className="w-12 h-12 object-cover rounded"
                      />
                    </td>
                    <td>
                      <div className="font-medium">{product.name}</div>
                      <div className="text-sm text-gray-500 md:hidden">
                        ${product.price.toFixed(2)}
                      </div>
                    </td>
                    <td className="hidden md:table-cell">${product.price.toFixed(2)}</td>
                    <td className="hidden md:table-cell">
                      {product.categories.map((cat, index) => (
                        <span key={index} className="inline-block bg-gray-100 rounded-full px-2 py-1 text-xs mr-1 mb-1">
                          {cat}
                        </span>
                      ))}
                    </td>
                    <td className="hidden md:table-cell">{product.variants}</td>
                    <td className="hidden md:table-cell">
                      <span className={`badge ${product.isActive ? 'badge-success' : 'badge-danger'}`}>
                        {product.isActive ? 'Active' : 'Inactive'}
                      </span>
                    </td>
                    <td>
                      <div className="flex items-center gap-2">
                        <Link to={`/products/${product.id}`} className="p-1 text-blue-600 hover:text-blue-800">
                          <Eye size={18} />
                        </Link>
                        <Link to={`/products/edit/${product.id}`} className="p-1 text-amber-600 hover:text-amber-800">
                          <Edit size={18} />
                        </Link>
                        <button 
                          onClick={() => handleDelete(product.id)}
                          className="p-1 text-red-600 hover:text-red-800"
                        >
                          <Trash2 size={18} />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="7" className="text-center py-4">No products found</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  )
}

export default Products