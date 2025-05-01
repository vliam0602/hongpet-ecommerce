import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { Search, Eye } from 'lucide-react'

// Mock data for orders
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

function Orders() {
  const [orders, setOrders] = useState([])
  const [loading, setLoading] = useState(true)
  const [searchTerm, setSearchTerm] = useState('')
  const [statusFilter, setStatusFilter] = useState('all')
  
  useEffect(() => {
    // Simulate API call
    setTimeout(() => {
      setOrders(mockOrders)
      setLoading(false)
    }, 500)
  }, [])
  
  const filteredOrders = orders.filter(order => {
    const matchesSearch = 
      order.id.includes(searchTerm) ||
      order.customerName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      order.customerPhone.includes(searchTerm)
    
    if (statusFilter === 'all') return matchesSearch
    return matchesSearch && order.status.toLowerCase() === statusFilter.toLowerCase()
  })
  
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
    return <div className="flex justify-center items-center h-full">Loading orders...</div>
  }
  
  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Orders</h1>
      </div>
      
      <div className="card">
        <div className="flex flex-col md:flex-row gap-4 mb-6">
          <div className="relative flex-1">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" size={18} />
            <input
              type="text"
              placeholder="Search orders..."
              className="input-field pl-10"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
          <div className="flex flex-wrap gap-2">
            <button 
              className={`btn ${statusFilter === 'all' ? 'btn-primary' : 'btn-dark'}`}
              onClick={() => setStatusFilter('all')}
            >
              All
            </button>
            <button 
              className={`btn ${statusFilter === 'pending' ? 'btn-primary' : 'btn-dark'}`}
              onClick={() => setStatusFilter('pending')}
            >
              Pending
            </button>
            <button 
              className={`btn ${statusFilter === 'processing' ? 'btn-primary' : 'btn-dark'}`}
              onClick={() => setStatusFilter('processing')}
            >
              Processing
            </button>
            <button 
              className={`btn ${statusFilter === 'completed' ? 'btn-primary' : 'btn-dark'}`}
              onClick={() => setStatusFilter('completed')}
            >
              Completed
            </button>
            <button 
              className={`btn ${statusFilter === 'cancelled' ? 'btn-primary' : 'btn-dark'}`}
              onClick={() => setStatusFilter('cancelled')}
            >
              Cancelled
            </button>
          </div>
        </div>
        
        <div className="table-container">
          <table className="table">
            <thead>
              <tr>
                <th>Order ID</th>
                <th>Customer</th>
                <th className="hidden md:table-cell">Date</th>
                <th className="hidden md:table-cell">Total</th>
                <th>Status</th>
                <th className="hidden md:table-cell">Payment</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {filteredOrders.length > 0 ? (
                filteredOrders.map((order) => (
                  <tr key={order.id}>
                    <td>#{order.id}</td>
                    <td>
                      <div className="font-medium">{order.customerName}</div>
                      <div className="text-sm text-gray-500">{order.customerPhone}</div>
                    </td>
                    <td className="hidden md:table-cell">{order.createdDate}</td>
                    <td className="hidden md:table-cell">${order.totalAmount.toFixed(2)}</td>
                    <td>
                      <span className={`badge ${getStatusBadgeClass(order.status)}`}>
                        {order.status}
                      </span>
                    </td>
                    <td className="hidden md:table-cell">{order.paymentMethod}</td>
                    <td>
                      <Link to={`/orders/${order.id}`} className="p-1 text-blue-600 hover:text-blue-800">
                        <Eye size={18} />
                      </Link>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="7" className="text-center py-4">No orders found</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  )
}

export default Orders