import { useState, useEffect } from 'react'
import { Link, useParams, useNavigate } from 'react-router-dom'
import { ArrowLeft, Edit, Trash2, Tag, Package, Star } from 'lucide-react'
import productService from '../../services/productService'

function ProductDetail() {
  const { id } = useParams()
  const navigate = useNavigate()
  const [product, setProduct] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [activeImageIndex, setActiveImageIndex] = useState(0)
  
  useEffect(() => {
    const fetchProductDetail = async () => {
      try {
        setLoading(true)
        const data = await productService.getProductDetail(id)
        setProduct(data)
        setError(null)
      } catch (err) {
        console.error('Error fetching product details:', err)
        setError('Failed to load product details. Please try again later.')
        setProduct(null)
      } finally {
        setLoading(false)
      }
    }
    
    fetchProductDetail()
  }, [id])
  
  const handleDelete = async () => {
    if (window.confirm('Are you sure you want to delete this product?')) {
      try {
        await productService.deleteProduct(id)
        navigate('/products')
      } catch (err) {
        console.error('Error deleting product:', err)
        alert('Failed to delete product. Please try again.')
      }
    }
  }
  
  if (loading) {
    return <div className="flex justify-center items-center h-full">Loading product details...</div>
  }
  
  if (error) {
    return (
      <div className="space-y-6">
        <div className="flex items-center gap-2">
          <Link to="/products" className="text-gray-500 hover:text-black">
            <ArrowLeft size={20} />
          </Link>
          <h1 className="text-2xl font-bold">Product Details</h1>
        </div>
        
        <div className="card p-8 text-center">
          <h2 className="text-xl font-semibold text-red-500">Error</h2>
          <p className="mt-2 text-gray-500">{error}</p>
          <Link to="/products" className="btn btn-primary mt-4 inline-block">
            Back to Products
          </Link>
        </div>
      </div>
    )
  }
  
  if (!product) {
    return (
      <div className="space-y-6">
        <div className="flex items-center gap-2">
          <Link to="/products" className="text-gray-500 hover:text-black">
            <ArrowLeft size={20} />
          </Link>
          <h1 className="text-2xl font-bold">Product Details</h1>
        </div>
        
        <div className="card p-8 text-center">
          <h2 className="text-xl font-semibold text-gray-500">Product not found</h2>
          <p className="mt-2 text-gray-500">The product you are looking for does not exist or has been removed.</p>
          <Link to="/products" className="btn btn-primary mt-4 inline-block">
            Back to Products
          </Link>
        </div>
      </div>
    )
  }
  
  // Calculate variants range price
  const priceRange = product.variants && product.variants.length > 0 
    ? {
        min: Math.min(...product.variants.map(v => v.price)),
        max: Math.max(...product.variants.map(v => v.price))
      }
    : { min: product.price, max: product.price };
  
  return (
    <div className="space-y-6">
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div className="flex items-center gap-2">
          <Link to="/products" className="text-gray-500 hover:text-black">
            <ArrowLeft size={20} />
          </Link>
          <h1 className="text-2xl font-bold">{product.name}</h1>
        </div>
        
        <div className="flex items-center gap-2">
          <Link 
            to={`/products/edit/${product.id}`} 
            className="btn btn-dark flex items-center gap-2"
          >
            <Edit size={18} />
            Edit
          </Link>
          <button 
            className="btn bg-red-600 text-white hover:bg-red-700 flex items-center gap-2"
            onClick={handleDelete}
          >
            <Trash2 size={18} />
            Delete
          </button>
        </div>
      </div>
      
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Product Images */}
        <div className="lg:col-span-2 space-y-6">
          <div className="card">
            <div className="relative aspect-video bg-gray-100 rounded-lg overflow-hidden mb-4">
              <img 
                src={(product.images && product.images.length > 0) 
                  ? product.images[activeImageIndex]?.imageUrl || product.thumbnailUrl
                  : product.thumbnailUrl || "/placeholder.svg"} 
                alt={product.name} 
                className="w-full h-full object-contain"
              />
            </div>
            
            {product.images && product.images.length > 0 && (
              <div className="flex gap-2 overflow-x-auto pb-2">
                {product.images.map((image, index) => (
                  <button 
                    key={image.id}
                    className={`w-20 h-20 rounded-lg overflow-hidden border-2 ${
                      index === activeImageIndex ? 'border-primary' : 'border-transparent'
                    }`}
                    onClick={() => setActiveImageIndex(index)}
                  >
                    <img 
                      src={image.imageUrl || "/placeholder.svg"} 
                      alt={image.name || `Image ${index + 1}`} 
                      className="w-full h-full object-cover"
                    />
                  </button>
                ))}
              </div>
            )}
          </div>
          
          {/* Product Description */}
          <div className="card">
            <h2 className="text-lg font-semibold mb-4">Description</h2>
            {product.description ? (
              <div dangerouslySetInnerHTML={{ __html: product.description }} />
            ) : (
              <p className="text-gray-500">No description available.</p>
            )}
          </div>                   
        </div>
        
        {/* Product Details */}
        <div className="space-y-6">
          <div className="card">
            <h2 className="text-lg font-semibold mb-4">Product Details</h2>
            
            <div className="space-y-4">
              <div>
                <h3 className="text-sm font-medium text-gray-500">Status</h3>
                <div className="mt-1">
                  <span className={`badge ${product.isActive ? 'badge-success' : 'badge-danger'}`}>
                    {product.isActive ? 'Active' : 'Inactive'}
                  </span>
                </div>
              </div>
              
              <div>
                <h3 className="text-sm font-medium text-gray-500">Price Range</h3>
                <p className="mt-1 font-semibold">
                  {priceRange.min === priceRange.max ? (
                    new Intl.NumberFormat('vi-VN', {
                      style: 'currency',
                      currency: 'VND',
                    }).format(priceRange.min)
                  ) : (
                    <>
                      {new Intl.NumberFormat('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                      }).format(priceRange.min)} - 
                      {new Intl.NumberFormat('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                      }).format(priceRange.max)}
                    </>
                  )}
                </p>
              </div>
              
              {product.categories && product.categories.length > 0 && (
                <div>
                  <h3 className="text-sm font-medium text-gray-500">Categories</h3>
                  <div className="mt-1 flex flex-wrap gap-2">
                    {product.categories.map((category, index) => (
                      <div key={index} className="flex items-center gap-1 bg-gray-100 rounded-full px-3 py-1 text-sm">
                        <Tag size={14} />
                        <span>{category.name}</span>
                      </div>
                    ))}
                  </div>
                </div>
              )}
              
              <div>
                <h3 className="text-sm font-medium text-gray-500">Created Date</h3>
                <p className="mt-1">
                  {new Date(product.createdDate).toLocaleDateString()}
                </p>
              </div>
            </div>
          </div>
          
          {/* Variants */}
          {product.variants && product.variants.length > 0 && (
            <div className="card">
              <h2 className="text-lg font-semibold mb-4">
                Variants ({product.variants.length})
              </h2>
              
              <div className="space-y-4">
                {product.variants.map((variant) => (
                  <div key={variant.id} className="border border-gray-200 rounded-lg p-4">
                    <div className="flex gap-4">                                            
                      <div className="flex-1">
                        <div className="flex justify-between">
                          <div className="font-medium">
                            {variant.attributeValues && variant.attributeValues.map((attr, index) => (
                              <span key={index}>
                                {attr.value}
                                {index < variant.attributeValues.length - 1 ? ', ' : ''}
                              </span>
                            ))}
                            {(!variant.attributeValues || variant.attributeValues.length === 0) && 'Default'}
                          </div>
                          <span className={`badge ${variant.isActive ? 'badge-success' : 'badge-danger'}`}>
                            {variant.isActive ? 'Active' : 'Inactive'}
                          </span>
                        </div>
                        
                        <div className="mt-2 flex justify-between">
                          <div className="flex items-center gap-2 text-gray-600">
                            <Package size={16} />
                            <span>Stock: {variant.stock}</span>
                          </div>
                          <span className="font-semibold">
                            {new Intl.NumberFormat('vi-VN', {
                              style: 'currency',
                              currency: 'VND',
                            }).format(variant.price)}
                          </span>
                        </div>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  )
}

export default ProductDetail