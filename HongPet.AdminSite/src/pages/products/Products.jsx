import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { Plus, Search, Edit, Trash2, Eye } from 'lucide-react'
import productService from '../../services/productService'

function Products() {
  const [products, setProducts] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [searchTerm, setSearchTerm] = useState('')
  const [filterActive, setFilterActive] = useState('all')
  const [pagination, setPagination] = useState({
    pageIndex: 1,
    pageSize: 10,
    totalItems: 0,
    totalPages: 0
  })

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        setLoading(true)
        const data = await productService.getProducts(
          pagination.pageIndex,
          pagination.pageSize,
          searchTerm
        )
        
        setProducts(data.items || [])
        setPagination({
          pageIndex: data.currentPage,
          pageSize: data.pageSize,
          totalItems: data.totalCount,
          totalPages: data.totalPages
        })
        setError(null)
      } catch (err) {
        console.error('Error fetching products:', err)
        setError('Failed to load products. Please try again later.')
        setProducts([])
      } finally {
        setLoading(false)
      }
    }
    
    // Debounce search input
    const timer = setTimeout(() => {
      fetchProducts()
    }, 500)
    
    return () => clearTimeout(timer)
  }, [searchTerm, pagination.pageIndex, pagination.pageSize])

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this product?')) {
      try {
        setLoading(true)
        await productService.deleteProduct(id)
        // Refresh the product list after deletion
        setProducts(products.filter(product => product.id !== id))
      } catch (err) {
        console.error('Error deleting product:', err)
        alert('Failed to delete product. Please try again.')
      } finally {
        setLoading(false)
      }
    }
  }

  const filteredProducts = products.filter(product => {
    if (filterActive === 'all') return true
    if (filterActive === 'active') return product.isActive
    if (filterActive === 'inactive') return !product.isActive
    
    return true
  })

  if (loading && products.length === 0) {
    return <div className="flex justify-center items-center h-full">Loading products...</div>
  }

  if (error) {
    return <div className="flex justify-center items-center h-full text-red-500">{error}</div>
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
          {/* Search box */}
          <div className="relative flex-1">
            <Search className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400" size={18} />
            <input
              type="text"
              placeholder="Search products..."
              className="input-field pl-10"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
          {/* Filter by active */}
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

        {loading && products.length > 0 && 
        <div className="text-center py-4">Refreshing data...</div>}

        <div className="table-container">
          <table className="table">
            <thead>
              <tr>
                <th>Image</th>
                <th>Name</th>
                <th className="hidden md:table-cell">Price</th>
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
                        {new Intl.NumberFormat('vi-VN', {
                          style: 'currency',
                          currency: 'VND',
                        }).format(product.price)}
                      </div>
                    </td>
                    <td className="hidden md:table-cell">
                      {new Intl.NumberFormat('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                      }).format(product.price)}
                    </td>                    
                    <td className="hidden md:table-cell">{product.totalVariants || 0}</td>
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
      </div>
    </div>
  )
}

export default Products