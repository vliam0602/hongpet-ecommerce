import { useState, useEffect } from 'react'
import { Link, useParams } from 'react-router-dom'
import { ArrowLeft, Truck, CreditCard, MapPin, User, Phone } from 'lucide-react'
import orderService from '../../services/orderService'

function OrderDetail() {
  const { id } = useParams()
  const [order, setOrder] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  
  useEffect(() => {
    const fetchOrderDetail = async () => {
      try {
        setLoading(true)
        const data = await orderService.getOrderDetail(id)
        setOrder(data)
        console.log(order);
        setError(null)
      } catch (err) {
        console.error('Error fetching order details:', err)
        setError('Failed to load order details. Please try again later.')
        setOrder(null)
      } finally {
        setLoading(false)
      }
    }
    
    fetchOrderDetail()
  }, [id])
  
  const getStatusBadgeClass = (status) => {
    switch (status?.toLowerCase()) {
      case 'completed':
        return 'badge-success'
      case 'processing':
        return 'badge-warning'
      case 'pending':
        return 'badge-warning'
      case 'cancelled':
        return 'badge-danger'
      default:
        return 'bg-gray-100 text-gray-800'
    }
  }
  
  if (loading) {
    return <div className="flex justify-center items-center h-full">Loading order details...</div>
  }
  
  if (error) {
    return (
      <div className="space-y-6">
        <div className="flex items-center gap-2">
          <Link to="/orders" className="text-gray-500 hover:text-black">
            <ArrowLeft size={20} />
          </Link>
          <h1 className="text-2xl font-bold">Order Details</h1>
        </div>
        
        <div className="card p-8 text-center">
          <h2 className="text-xl font-semibold text-red-500">Error</h2>
          <p className="mt-2 text-gray-500">{error}</p>
          <Link to="/orders" className="btn btn-primary mt-4 inline-block">
            Back to Orders
          </Link>
        </div>
      </div>
    )
  }
  
  if (!order) {
    return (
      <div className="space-y-6">
        <div className="flex items-center gap-2">
          <Link to="/orders" className="text-gray-500 hover:text-black">
            <ArrowLeft size={20} />
          </Link>
          <h1 className="text-2xl font-bold">Order Details</h1>
        </div>
        
        <div className="card p-8 text-center">
          <h2 className="text-xl font-semibold text-gray-500">Order not found</h2>
          <p className="mt-2 text-gray-500">The order you are looking for does not exist or has been removed.</p>
          <Link to="/orders" className="btn btn-primary mt-4 inline-block">
            Back to Orders
          </Link>
        </div>
      </div>
    )
  }
  
  // Calculate subtotal
  const subtotal = order.orderItems.reduce((sum, item) => sum + (item.price * item.quantity), 0)
  
  // Shipping cost and tax
  const shippingCost = order.shippingFee || 0
  const tax = order.tax || 0
  
  return (
    <div className="space-y-6">
      <div className="flex items-center gap-2">
        <Link to="/orders" className="text-gray-500 hover:text-black">
          <ArrowLeft size={20} />
        </Link>
        <h1 className="text-2xl font-bold">Order #{order.id}</h1>
      </div>
      
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Order Summary */}
        <div className="lg:col-span-2 space-y-6">
          <div className="card">
            <div className="flex justify-between items-center mb-4">
              <h2 className="text-lg font-semibold">Order Summary</h2>
              <span className={`badge ${getStatusBadgeClass(order.status)}`}>
                {order.status}
              </span>
            </div>
            
            <div className="space-y-4">
              <div className="flex items-center gap-2 text-gray-600">
                <CreditCard size={18} />
                <span>Payment Method: {order.paymentMethod}</span>
              </div>
              
              <div className="flex items-center gap-2 text-gray-600">
                <Truck size={18} />
                <span>Order Date: {new Date(order.createdDate).toLocaleDateString('vi-VN')}</span>
              </div>
            </div>
          </div>
          
          {/* Order Items */}
          <div className="card">
            <h2 className="text-lg font-semibold mb-4">Order Items</h2>
            
            <div className="space-y-4">
              {order.orderItems.map((item, index) => (
                <div key={index} className="flex gap-4 py-4 border-b border-gray-100 last:border-0">
                  <img 
                    src={item.thumbnailImageUrl || "/placeholder.svg"} 
                    alt={item.productName} 
                    className="w-16 h-16 object-cover rounded"
                  />
                  
                  <div className="flex-1">
                    <h3 className="font-medium">{item.productName}</h3>
                    
                    {item.attributeValues && item.attributeValues.length > 0 && (
                      <div className="mt-1 space-x-2">
                        {item.attributeValues.map((attr, attrIndex) => (
                          <span key={attrIndex} className="text-sm text-gray-500">
                            {attr.attribute}: {attr.value}
                          </span>
                        ))}
                      </div>
                    )}
                    
                    <div className="mt-2 flex justify-between">
                      <span className="text-sm text-gray-500">
                        {new Intl.NumberFormat('vi-VN', {
                          style: 'currency',
                          currency: 'VND',
                        }).format(item.price)} x {item.quantity}
                      </span>
                      <span className="font-medium">
                        {new Intl.NumberFormat('vi-VN', {
                          style: 'currency',
                          currency: 'VND',
                        }).format(item.price * item.quantity)}
                      </span>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
        
        {/* Customer and Payment Info */}
        <div className="space-y-6">
          <div className="card">
            <h2 className="text-lg font-semibold mb-4">Customer Information</h2>
            
            <div className="space-y-4">
              <div className="flex items-center gap-2 text-gray-600">
                <User size={18} />
                <span>{order.customerName}</span>
              </div>
              
              <div className="flex items-center gap-2 text-gray-600">
                <Phone size={18} />
                <span>{order.customerPhone}</span>
              </div>
              
              <div className="flex items-start gap-2 text-gray-600">
                <MapPin size={18} className="flex-shrink-0 mt-1" />
                <span>{order.shippingAddress}</span>
              </div>
            </div>
          </div>
          
          <div className="card">
            <h2 className="text-lg font-semibold mb-4">Payment Summary</h2>
            
            <div className="space-y-2">
              <div className="flex justify-between">
                <span className="text-gray-600">Subtotal</span>
                <span>
                  {new Intl.NumberFormat('vi-VN', {
                    style: 'currency',
                    currency: 'VND',
                  }).format(subtotal)}
                </span>
              </div>
              
              <div className="flex justify-between">
                <span className="text-gray-600">Shipping</span>
                <span>
                  {new Intl.NumberFormat('vi-VN', {
                    style: 'currency',
                    currency: 'VND',
                  }).format(shippingCost)}
                </span>
              </div>
              
              <div className="flex justify-between">
                <span className="text-gray-600">Tax</span>
                <span>
                  {new Intl.NumberFormat('vi-VN', {
                    style: 'currency',
                    currency: 'VND',
                  }).format(tax)}
                </span>
              </div>
              
              <div className="border-t border-gray-200 my-2 pt-2">
                <div className="flex justify-between font-semibold">
                  <span>Total</span>
                  <span>
                    {new Intl.NumberFormat('vi-VN', {
                      style: 'currency',
                      currency: 'VND',
                    }).format(order.totalAmount)}
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default OrderDetail