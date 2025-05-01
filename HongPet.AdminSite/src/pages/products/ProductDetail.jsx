import { useState, useEffect } from 'react'
import { Link, useParams } from 'react-router-dom'
import { ArrowLeft, Edit, Trash2, Tag, Package } from 'lucide-react'

// Mock data for products (same as in Products.jsx)
const mockProducts = [
  { 
    id: '1', 
    name: 'Premium Dog Food', 
    description: '<p>High-quality dog food for all breeds. Made with real meat and vegetables to provide complete nutrition for your dog.</p><h2>Features</h2><ul><li>No artificial preservatives</li><li>Rich in protein</li><li>Supports healthy digestion</li></ul>',
    brief: 'High-quality dog food for all breeds',
    price: 29.99,
    thumbnailUrl: 'https://placehold.co/400x400',
    isActive: true,
    categories: ['Pet Food', 'Dogs'],
    variants: [
      {
        id: '1',
        price: 29.99,
        stock: 50,
        thumbnailUrl: 'https://placehold.co/400x400',
        isActive: true,
        attributes: [
          { name: 'Size', value: 'Small (2kg)' }
        ]
      },
      {
        id: '2',
        price: 49.99,
        stock: 30,
        thumbnailUrl: 'https://placehold.co/400x400',
        isActive: true,
        attributes: [
          { name: 'Size', value: 'Medium (5kg)' }
        ]
      },
      {
        id: '3',
        price: 79.99,
        stock: 20,
        thumbnailUrl: 'https://placehold.co/400x400',
        isActive: true,
        attributes: [
          { name: 'Size', value: 'Large (10kg)' }
        ]
      }
    ],
    images: [
      { id: '1', name: 'Front view', imageUrl: 'https://placehold.co/800x600' },
      { id: '2', name: 'Side view', imageUrl: 'https://placehold.co/800x600' },
      { id: '3', name: 'Ingredients', imageUrl: 'https://placehold.co/800x600' }
    ],
    createdDate: '2025-04-15'
  },
  { 
    id: '2', 
    name: 'Cat Toy Set', 
    description: '<p>Interactive toys for cats. Includes feather wands, balls, and mice toys to keep your cat entertained.</p><h2>Benefits</h2><ul><li>Stimulates natural hunting instincts</li><li>Provides exercise</li><li>Reduces boredom</li></ul>',
    brief: 'Interactive toys for cats',
    price: 24.99,
    thumbnailUrl: 'https://placehold.co/400x400',
    isActive: true,
    categories: ['Toys', 'Cats'],
    variants: [
      {
        id: '4',
        price: 24.99,
        stock: 40,
        thumbnailUrl: 'https://placehold.co/400x400',
        isActive: true,
        attributes: [
          { name: 'Type', value: 'Basic Set' }
        ]
      },
      {
        id: '5',
        price: 34.99,
        stock: 25,
        thumbnailUrl: 'https://placehold.co/400x400',
        isActive: true,
        attributes: [
          { name: 'Type', value: 'Deluxe Set' }
        ]
      }
    ],
    images: [
      { id: '4', name: 'Full set', imageUrl: 'https://placehold.co/800x600' },
      { id: '5', name: 'In use', imageUrl: 'https://placehold.co/800x600' }
    ],
    createdDate: '2025-04-10'
  }
]

function ProductDetail() {
  const { id } = useParams()
  const [product, setProduct] = useState(null)
  const [loading, setLoading] = useState(true)
  const [activeImageIndex, setActiveImageIndex] = useState(0)
  
  useEffect(() => {
    // Simulate API call
    setTimeout(() => {
      const foundProduct = mockProducts.find(product => product.id === id)
      setProduct(foundProduct || null)
      setLoading(false)
    }, 500)
  }, [id])
  
  const handleDelete = () => {
    if (window.confirm('Are you sure you want to delete this product?')) {
      // In a real app, you would call an API to delete the product
      alert('Product deleted successfully!')
      // Navigate back to products page
      window.location.href = '/products'
    }
  }
  
  if (loading) {
    return <div className="flex justify-center items-center h-full">Loading product details...</div>
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
                src={product.images[activeImageIndex]?.imageUrl || product.thumbnailUrl} 
                alt={product.name} 
                className="w-full h-full object-contain"
              />
            </div>
            
            {product.images.length > 0 && (
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
            <div dangerouslySetInnerHTML={{ __html: product.description }} />
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
                  ${Math.min(...product.variants.map(v => v.price)).toFixed(2)} - 
                  ${Math.max(...product.variants.map(v => v.price)).toFixed(2)}
                </p>
              </div>
              
              <div>
                <h3 className="text-sm font-medium text-gray-500">Categories</h3>
                <div className="mt-1 flex flex-wrap gap-2">
                  {product.categories.map((category, index) => (
                    <div key={index} className="flex items-center gap-1 bg-gray-100 rounded-full px-3 py-1 text-sm">
                      <Tag size={14} />
                      <span>{category}</span>
                    </div>
                  ))}
                </div>
              </div>
              
              <div>
                <h3 className="text-sm font-medium text-gray-500">Created Date</h3>
                <p className="mt-1">{product.createdDate}</p>
              </div>
            </div>
          </div>
          
          {/* Variants */}
          <div className="card">
            <h2 className="text-lg font-semibold mb-4">Variants ({product.variants.length})</h2>
            
            <div className="space-y-4">
              {product.variants.map((variant) => (
                <div key={variant.id} className="border border-gray-200 rounded-lg p-4">
                  <div className="flex gap-4">
                    {variant.thumbnailUrl && (
                      <img 
                        src={variant.thumbnailUrl || "/placeholder.svg"} 
                        alt="Variant thumbnail" 
                        className="w-16 h-16 object-cover rounded"
                      />
                    )}
                    
                    <div className="flex-1">
                      <div className="flex justify-between">
                        <div className="font-medium">
                          {variant.attributes.map((attr, index) => (
                            <span key={index}>
                              {attr.value}
                              {index < variant.attributes.length - 1 ? ', ' : ''}
                            </span>
                          ))}
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
                        <span className="font-semibold">${variant.price.toFixed(2)}</span>
                      </div>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default ProductDetail