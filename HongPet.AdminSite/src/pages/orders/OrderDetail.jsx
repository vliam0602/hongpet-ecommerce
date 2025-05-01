import { useState, useEffect } from 'react'
import { Link, useParams } from 'react-router-dom'
import { ArrowLeft, Truck, CreditCard, MapPin, User, Phone } from 'lucide-react'

// Mock data for orders (same as in Orders.jsx)
const mockOrders = [
  {
    id: '1',
    customerId: '1',
    customerName: 'John Doe',
    customerPhone: '(123) 456-7890',
    shippingAddress: '123 Main St, City, Country',
    totalAmount: 129.99,
    status: 'Completed',
    paymentMethod: 'Credit Card',
    createdDate: '2025-04-30',
    orderItems: [
      {
        productId: '1',
        variantId: '1',
        productName: 'Premium Dog Food',
        thumbnailImageUrl: 'https://placehold.co/100x100',
        attributeValues: [
          { id: '1', attribute: 'Size', value: 'Large' }
        ],
        quantity: 2,
        price: 29.99
      },
      {
        productId: '3',
        variantId: '5',
        productName: 'Dog Collar',
        thumbnailImageUrl: 'https://placehold.co/100x100',
        attributeValues: [
          { id: '2', attribute: 'Size', value: 'Medium' },
          { id: '3', attribute: 'Color', value: 'Red' }
        ],
        quantity: 1,
        price: 19.99
      }
    ]
  },
  {
    id: '2',
    customerId: '2',
    customerName: 'Jane Smith',
    customerPhone: '(234) 567-8901',
    shippingAddress: '456 Oak Ave, Town, Country',
    totalAmount: 85.50,
    status: 'Processing',
    paymentMethod: 'PayPal',
    createdDate: '2025-04-29',
    orderItems: [
      {
        productId: '2',
        variantId: '3',
        productName: 'Cat Toy Set',
        thumbnailImageUrl: 'https://placehold.co/100x100',
        attributeValues: [],
        quantity: 1,
        price: 24.99
      },
      {
        productId: '4',
        variantId: '7',
        productName: 'Cat Food',
        thumbnailImageUrl: 'https://placehold.co/100x100',
        attributeValues: [
          { id: '4', attribute: 'Size', value: 'Small' }
        ],
        quantity: 2,
        price: 19.99
      }
    ]
  },
  {
    id: '3',
    customerId: '3',
    customerName: 'Robert Johnson',
    customerPhone: '(345) 678-9012',
    shippingAddress: '789 Pine Rd, Village, Country',
    totalAmount: 210.75,
    status: 'Pending',
    paymentMethod: 'Credit Card',
    createdDate: '2025-04-28',
    orderItems: [
      {
        productId: '5',
        variantId: '9',
        productName: 'Bird Cage',
        thumbnailImageUrl: 'https://placehold.co/100x100',
        attributeValues: [
          { id: '5', attribute: 'Size', value: 'Large' }
        ],
        quantity: 1,
        price: 149.99
      },
      {
        productId: '6',
        variantId: '11',
        productName: 'Bird Food',
        thumbnailImageUrl: 'https://placehold.co/100x100',
        attributeValues: [],
        quantity: 2,
        price: 29.99
      }
    ]
  },
  {
    id: '4',
    customerId: '4',
    customerName: 'Emily Davis',
    customerPhone: '(456) 789-0123',
    shippingAddress: '101 Maple Dr, Suburb, Country',
    totalAmount: 65.99,
    status: 'Completed',
    paymentMethod: 'PayPal',
    createdDate: '2025-04-27',
    orderItems: [
      {
        productId: '7',
        variantId: '13',
        productName: 'Hamster Wheel',
        thumbnailImageUrl: 'https://placehold.co/100x100',
        attributeValues: [],
        quantity: 1,
        price: 15.99
      },
      {
        productId: '8',
        variantId: '15',
        productName: 'Hamster Food',
        thumbnailImageUrl: 'https://placehold.co/100x100',
        attributeValues: [],
        quantity: 2,
        price: 24.99
      }
    ]
  },
  {
    id: '5',
    customerId: '5',
    customerName: 'Michael Wilson',
    customerPhone: '(567) 890-1234',
    shippingAddress: '202 Cedar Ln, District, Country',
    totalAmount: 45.99,
    status: 'Cancelled',
    paymentMethod: 'Credit Card',
    createdDate: '2025-04-26',
    orderItems: [
      {
        productId: '9',
        variantId: '17',
        productName: 'Fish Tank Filter',
        thumbnailImageUrl: 'https://placehold.co/100x100',
        attributeValues: [],
        quantity: 1,
        price: 45.99
      }
    ]
  }
]

function OrderDetail() {
  const { id } = useParams()
  const [order, setOrder] = useState(null)
  const [loading, setLoading] = useState(true)
  
  useEffect(() => {
    // Simulate API call
    setTimeout(() => {
      const foundOrder = mockOrders.find(order => order.id === id)
      setOrder(foundOrder || null)
      setLoading(false)
    }, 500)
  }, [id])
  
  const getStatusBadgeClass = (status) => {
    switch (status.toLowerCase()) {
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
  
  // Assume shipping cost and tax for demo
  const shippingCost = 10.00
  const taxRate = 0.08
  const tax = subtotal * taxRate
  
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
                <span>Order Date: {order.createdDate}</span>
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
                    
                    {item.attributeValues.length > 0 && (
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
                        ${item.price.toFixed(2)} x {item.quantity}
                      </span>
                      <span className="font-medium">
                        ${(item.price * item.quantity).toFixed(2)}
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
                <span>${subtotal.toFixed(2)}</span>
              </div>
              
              <div className="flex justify-between">
                <span className="text-gray-600">Shipping</span>
                <span>${shippingCost.toFixed(2)}</span>
              </div>
              
              <div className="flex justify-between">
                <span className="text-gray-600">Tax</span>
                <span>${tax.toFixed(2)}</span>
              </div>
              
              <div className="border-t border-gray-200 my-2 pt-2">
                <div className="flex justify-between font-semibold">
                  <span>Total</span>
                  <span>${order.totalAmount.toFixed(2)}</span>
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