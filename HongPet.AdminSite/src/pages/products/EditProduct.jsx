import { useState, useEffect, useRef } from 'react'
import { Link, useParams, useNavigate } from 'react-router-dom'
import { ArrowLeft, ArrowRight, Check, Upload, Plus, X, ChevronDown } from 'lucide-react'
import { useEditor, EditorContent } from '@tiptap/react'
import StarterKit from '@tiptap/starter-kit'
import Image from '@tiptap/extension-image'
import { Link as TiptapLink } from '@tiptap/extension-link'

// Mock data for products (same as in ProductDetail.jsx)
const mockProducts = [
  { 
    id: '1', 
    name: 'Premium Dog Food', 
    description: '<p>High-quality dog food for all breeds. Made with real meat and vegetables to provide complete nutrition for your dog.</p><h2>Features</h2><ul><li>No artificial preservatives</li><li>Rich in protein</li><li>Supports healthy digestion</li></ul>',
    brief: 'High-quality dog food for all breeds',
    price: 29.99,
    thumbnailUrl: 'https://placehold.co/400x400',
    isActive: true,
    categories: ['1', '2'], // Using IDs for categories
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
    categories: ['3', '4'], // Using IDs for categories
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

// Mock data for categories
const mockCategories = [
  { id: '1', name: 'Pet Food' },
  { id: '2', name: 'Dogs' },
  { id: '3', name: 'Toys' },
  { id: '4', name: 'Cats' },
  { id: '5', name: 'Accessories' },
  { id: '6', name: 'Grooming' },
  { id: '7', name: 'Health & Wellness' },
  { id: '8', name: 'Beds & Furniture' },
  { id: '9', name: 'Cages & Carriers' },
  { id: '10', name: 'Clothing' },
]

// Mock data for attributes
const mockAttributes = [
  { id: '1', name: 'Size' },
  { id: '2', name: 'Color' },
  { id: '3', name: 'Material' },
  { id: '4', name: 'Weight' },
  { id: '5', name: 'Age Group' },
]

function EditProduct() {
  const { id } = useParams()
  const navigate = useNavigate()
  const fileInputRef = useRef(null)
  const variantFileInputRef = useRef(null)
  const imagesFileInputRef = useRef(null)
  
  const [currentStep, setCurrentStep] = useState(1)
  const [showAttributeDropdown, setShowAttributeDropdown] = useState(false)
  const [loading, setLoading] = useState(true)
  const [productData, setProductData] = useState(null)
  
  useEffect(() => {
    // Simulate API call
    setTimeout(() => {
      const foundProduct = mockProducts.find(product => product.id === id)
      
      if (foundProduct) {
        setProductData({
          ...foundProduct,
          // Make a deep copy of variants to avoid reference issues
          variants: foundProduct.variants.map(variant => ({
            ...variant,
            attributes: [...variant.attributes]
          })),
          // Make a deep copy of images to avoid reference issues
          images: [...foundProduct.images]
        })
      }
      
      setLoading(false)
    }, 500)
  }, [id])
  
  // TipTap editor for rich text description
  const editor = useEditor({
    extensions: [
      StarterKit,
      Image,
      TiptapLink
    ],
    content: productData?.description || '',
    onUpdate: ({ editor }) => {
      setProductData(prev => ({
        ...prev,
        description: editor.getHTML()
      }))
    }
  })
  
  // Update editor content when productData changes
  useEffect(() => {
    if (editor && productData) {
      editor.commands.setContent(productData.description)
    }
  }, [editor, productData?.description])
  
  const steps = [
    { id: 1, name: 'General Information' },
    { id: 2, name: 'Categories' },
    { id: 3, name: 'Variants' },
    { id: 4, name: 'Images' }
  ]
  
  if (loading || !productData) {
    return <div className="flex justify-center items-center h-full">Loading product data...</div>
  }
  
  const handleProductThumbnailUpload = (e) => {
    const file = e.target.files[0]
    if (file) {
      // In a real app, you would upload this to a server
      // For now, we'll just create a local URL
      const imageUrl = URL.createObjectURL(file)
      setProductData(prev => ({
        ...prev,
        thumbnailUrl: imageUrl
      }))
    }
  }
  
  const handleVariantThumbnailUpload = (e, variantIndex) => {
    const file = e.target.files[0]
    if (file) {
      const imageUrl = URL.createObjectURL(file)
      setProductData(prev => {
        const updatedVariants = [...prev.variants]
        updatedVariants[variantIndex].thumbnailUrl = imageUrl
        return {
          ...prev,
          variants: updatedVariants
        }
      })
    }
  }
  
  const handleProductImagesUpload = (e) => {
    const files = Array.from(e.target.files)
    if (files.length > 0) {
      const newImages = files.map(file => ({
        id: Date.now() + Math.random().toString(36).substr(2, 9),
        name: file.name,
        imageUrl: URL.createObjectURL(file)
      }))
      
      setProductData(prev => ({
        ...prev,
        images: [...prev.images, ...newImages]
      }))
    }
  }
  
  const toggleCategory = (categoryId) => {
    setProductData(prev => {
      if (prev.categories.includes(categoryId)) {
        return {
          ...prev,
          categories: prev.categories.filter(id => id !== categoryId)
        }
      } else {
        return {
          ...prev,
          categories: [...prev.categories, categoryId]
        }
      }
    })
  }
  
  const addVariant = () => {
    setProductData(prev => ({
      ...prev,
      variants: [
        ...prev.variants,
        {
          id: Date.now().toString(),
          price: '',
          stock: '',
          thumbnailUrl: null,
          isActive: true,
          attributes: []
        }
      ]
    }))
  }
  
  const removeVariant = (variantIndex) => {
    setProductData(prev => ({
      ...prev,
      variants: prev.variants.filter((_, index) => index !== variantIndex)
    }))
  }
  
  const addAttributeToVariant = (variantIndex, attribute) => {
    setProductData(prev => {
      const updatedVariants = [...prev.variants]
      
      // Check if attribute already exists
      const attributeExists = updatedVariants[variantIndex].attributes.some(
        attr => attr.name === attribute.name
      )
      
      if (!attributeExists) {
        updatedVariants[variantIndex].attributes.push({
          name: attribute.name,
          value: ''
        })
      }
      
      return {
        ...prev,
        variants: updatedVariants
      }
    })
    
    setShowAttributeDropdown(false)
  }
  
  const updateAttributeValue = (variantIndex, attributeIndex, value) => {
    setProductData(prev => {
      const updatedVariants = [...prev.variants]
      updatedVariants[variantIndex].attributes[attributeIndex].value = value
      return {
        ...prev,
        variants: updatedVariants
      }
    })
  }
  
  const removeAttribute = (variantIndex, attributeIndex) => {
    setProductData(prev => {
      const updatedVariants = [...prev.variants]
      updatedVariants[variantIndex].attributes.splice(attributeIndex, 1)
      return {
        ...prev,
        variants: updatedVariants
      }
    })
  }
  
  const removeImage = (imageId) => {
    setProductData(prev => ({
      ...prev,
      images: prev.images.filter(image => image.id !== imageId)
    }))
  }
  
  const handleNext = () => {
    if (currentStep < steps.length) {
      setCurrentStep(currentStep + 1)
    } else {
      handleSubmit()
    }
  }
  
  const handlePrevious = () => {
    if (currentStep > 1) {
      setCurrentStep(currentStep - 1)
    }
  }
  
  const handleSubmit = () => {
    // In a real app, you would send this data to your API
    console.log('Submitting updated product data:', productData)
    
    // Navigate back to product detail page
    alert('Product updated successfully!')
    navigate(`/products/${id}`)
  }
  
  return (
    <div className="space-y-6">
      <div className="flex items-center gap-2">
        <Link to={`/products/${id}`} className="text-gray-500 hover:text-black">
          <ArrowLeft size={20} />
        </Link>
        <h1 className="text-2xl font-bold">Edit Product: {productData.name}</h1>
      </div>
      
      {/* Progress Steps */}
      <div className="flex items-center justify-between mb-8">
        {steps.map((step) => (
          <div 
            key={step.id} 
            className={`flex items-center ${step.id < steps.length ? 'flex-1' : ''}`}
          >
            <div 
              className={`w-8 h-8 rounded-full flex items-center justify-center ${
                step.id < currentStep 
                  ? 'bg-primary text-black' 
                  : step.id === currentStep 
                    ? 'bg-black text-white' 
                    : 'bg-gray-200 text-gray-500'
              }`}
            >
              {step.id < currentStep ? (
                <Check size={16} />
              ) : (
                step.id
              )}
            </div>
            <div 
              className={`ml-2 ${
                step.id === currentStep ? 'font-medium' : 'text-gray-500'
              }`}
            >
              {step.name}
            </div>
            {step.id < steps.length && (
              <div 
                className={`flex-1 h-1 mx-4 ${
                  step.id < currentStep ? 'bg-primary' : 'bg-gray-200'
                }`}
              />
            )}
          </div>
        ))}
      </div>
      
      <div className="card">
        {/* Step 1: General Information */}
        {currentStep === 1 && (
          <div>
            <h2 className="text-xl font-semibold mb-4">General Information</h2>
            <p className="text-gray-500 mb-6">Edit basic information about the product</p>
            
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div className="space-y-4 md:col-span-1">
                <div>
                  <label htmlFor="productName" className="block mb-1 font-medium">
                    Product Name <span className="text-red-500">*</span>
                  </label>
                  <input
                    id="productName"
                    type="text"
                    className="input-field"
                    placeholder="Enter product name"
                    value={productData.name}
                    onChange={(e) => setProductData({...productData, name: e.target.value})}
                    required
                  />
                </div>
                
                <div>
                  <label htmlFor="brief" className="block mb-1 font-medium">
                    Brief Description
                  </label>
                  <input
                    id="brief"
                    type="text"
                    className="input-field"
                    placeholder="Short description (displayed in lists)"
                    value={productData.brief}
                    onChange={(e) => setProductData({...productData, brief: e.target.value})}
                  />
                </div>
                
                <div className="flex items-center">
                  <label className="relative inline-flex items-center cursor-pointer">
                    <input 
                      type="checkbox" 
                      className="sr-only peer"
                      checked={productData.isActive}
                      onChange={() => setProductData({...productData, isActive: !productData.isActive})}
                    />
                    <div className="w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-primary"></div>
                    <span className="ms-3 text-sm font-medium">Active</span>
                  </label>
                </div>
              </div>
              
              <div className="space-y-4 md:col-span-1">
                <div>
                  <label className="block mb-1 font-medium">
                    Product Thumbnail
                  </label>
                  <div className="border border-dashed border-gray-300 rounded-lg p-4 text-center">
                    {productData.thumbnailUrl ? (
                      <div className="relative">
                        <img 
                          src={productData.thumbnailUrl || "/placeholder.svg"} 
                          alt="Product thumbnail" 
                          className="mx-auto h-40 object-contain"
                        />
                        <button 
                          className="absolute top-0 right-0 bg-red-500 text-white rounded-full p-1"
                          onClick={() => setProductData({...productData, thumbnailUrl: null})}
                        >
                          <X size={16} />
                        </button>
                      </div>
                    ) : (
                      <div 
                        className="flex flex-col items-center justify-center h-40 cursor-pointer"
                        onClick={() => fileInputRef.current.click()}
                      >
                        <Upload className="text-gray-400 mb-2" size={24} />
                        <p className="text-sm text-gray-500">Upload Thumbnail</p>
                        <p className="text-xs text-gray-400 mt-1">Click to browse</p>
                      </div>
                    )}
                    <input
                      type="file"
                      ref={fileInputRef}
                      className="hidden"
                      accept="image/*"
                      onChange={handleProductThumbnailUpload}
                    />
                  </div>
                </div>
              </div>
            </div>
            
            <div className="mt-6">
              <label htmlFor="description" className="block mb-1 font-medium">
                Full Description
              </label>
              <div className="border border-gray-300 rounded-lg overflow-hidden">
                <div className="bg-gray-50 border-b border-gray-300 p-2 flex gap-2">
                  <button
                    onClick={() => editor?.chain().focus().toggleBold().run()}
                    className={`p-1 rounded ${editor?.isActive('bold') ? 'bg-gray-200' : ''}`}
                  >
                    <span className="font-bold">B</span>
                  </button>
                  <button
                    onClick={() => editor?.chain().focus().toggleItalic().run()}
                    className={`p-1 rounded ${editor?.isActive('italic') ? 'bg-gray-200' : ''}`}
                  >
                    <span className="italic">I</span>
                  </button>
                  <button
                    onClick={() => editor?.chain().focus().toggleUnderline().run()}
                    className={`p-1 rounded ${editor?.isActive('underline') ? 'bg-gray-200' : ''}`}
                  >
                    <span className="underline">U</span>
                  </button>
                  <button
                    onClick={() => editor?.chain().focus().toggleHeading({ level: 1 }).run()}
                    className={`p-1 rounded ${editor?.isActive('heading', { level: 1 }) ? 'bg-gray-200' : ''}`}
                  >
                    <span className="font-bold">H1</span>
                  </button>
                  <button
                    onClick={() => editor?.chain().focus().toggleHeading({ level: 2 }).run()}
                    className={`p-1 rounded ${editor?.isActive('heading', { level: 2 }) ? 'bg-gray-200' : ''}`}
                  >
                    <span className="font-bold">H2</span>
                  </button>
                  <button
                    onClick={() => editor?.chain().focus().toggleBulletList().run()}
                    className={`p-1 rounded ${editor?.isActive('bulletList') ? 'bg-gray-200' : ''}`}
                  >
                    <span>•</span>
                  </button>
                  <button
                    onClick={() => editor?.chain().focus().toggleOrderedList().run()}
                    className={`p-1 rounded ${editor?.isActive('orderedList') ? 'bg-gray-200' : ''}`}
                  >
                    <span>1.</span>
                  </button>
                </div>
                <EditorContent editor={editor} className="p-4 min-h-[200px]" />
              </div>
            </div>
          </div>
        )}
        
        {/* Step 2: Categories */}
        {currentStep === 2 && (
          <div>
            <h2 className="text-xl font-semibold mb-4">Product Categories</h2>
            <p className="text-gray-500 mb-6">Assign categories to the product</p>
            
            {productData.categories.length === 0 && (
              <p className="text-primary mb-4">Please select at least one category</p>
            )}
            
            <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
              {mockCategories.map((category) => (
                <div key={category.id} className="flex items-center">
                  <input
                    type="checkbox"
                    id={`category-${category.id}`}
                    className="w-4 h-4 text-primary bg-gray-100 border-gray-300 rounded focus:ring-primary"
                    checked={productData.categories.includes(category.id)}
                    onChange={() => toggleCategory(category.id)}
                  />
                  <label
                    htmlFor={`category-${category.id}`}
                    className="ms-2 text-sm font-medium"
                  >
                    {category.name}
                  </label>
                </div>
              ))}
            </div>
          </div>
        )}
        
        {/* Step 3: Variants */}
        {currentStep === 3 && (
          <div>
            <h2 className="text-xl font-semibold mb-4">Product Variants</h2>
            <p className="text-gray-500 mb-6">Edit variants of the product</p>
            
            {productData.variants.map((variant, variantIndex) => (
              <div 
                key={variant.id} 
                className="border border-gray-200 rounded-lg p-4 mb-6"
              >
                <div className="flex justify-between items-center mb-4">
                  <h3 className="font-medium">Variant {variantIndex + 1}</h3>
                  {productData.variants.length > 1 && (
                    <button 
                      className="text-red-500 hover:text-red-700"
                      onClick={() => removeVariant(variantIndex)}
                    >
                      <X size={18} />
                    </button>
                  )}
                </div>
                
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <div className="space-y-4">
                    <div className="grid grid-cols-2 gap-4">
                      <div>
                        <label htmlFor={`price-${variant.id}`} className="block mb-1 font-medium">
                          Price ($)
                        </label>
                        <input
                          id={`price-${variant.id}`}
                          type="number"
                          step="0.01"
                          min="0"
                          className="input-field"
                          placeholder="0.00"
                          value={variant.price}
                          onChange={(e) => {
                            const updatedVariants = [...productData.variants]
                            updatedVariants[variantIndex].price = e.target.value
                            setProductData({...productData, variants: updatedVariants})
                          }}
                        />
                      </div>
                      
                      <div>
                        <label htmlFor={`stock-${variant.id}`} className="block mb-1 font-medium">
                          Stock
                        </label>
                        <input
                          id={`stock-${variant.id}`}
                          type="number"
                          min="0"
                          className="input-field"
                          placeholder="0"
                          value={variant.stock}
                          onChange={(e) => {
                            const updatedVariants = [...productData.variants]
                            updatedVariants[variantIndex].stock = e.target.value
                            setProductData({...productData, variants: updatedVariants})
                          }}
                        />
                      </div>
                    </div>
                    
                    <div>
                      <label className="block mb-1 font-medium">Attributes</label>
                      
                      {variant.attributes.map((attr, attrIndex) => (
                        <div key={attrIndex} className="flex items-center gap-2 mb-2">
                          <div className="bg-gray-100 px-2 py-1 rounded text-sm">
                            {attr.name}
                          </div>
                          <input
                            type="text"
                            className="input-field flex-1"
                            placeholder={`Enter ${attr.name}`}
                            value={attr.value}
                            onChange={(e) => updateAttributeValue(variantIndex, attrIndex, e.target.value)}
                          />
                          <button 
                            className="text-red-500 hover:text-red-700"
                            onClick={() => removeAttribute(variantIndex, attrIndex)}
                          >
                            <X size={16} />
                          </button>
                        </div>
                      ))}
                      
                      <div className="relative">
                        <button
                          className="btn btn-dark flex items-center gap-2 mt-2"
                          onClick={() => setShowAttributeDropdown(!showAttributeDropdown)}
                        >
                          <Plus size={16} />
                          Add Attribute
                          <ChevronDown size={16} />
                        </button>
                        
                        {showAttributeDropdown && (
                          <div className="absolute z-10 mt-1 w-full bg-white border border-gray-200 rounded-md shadow-lg">
                            {mockAttributes.map((attr) => (
                              <button
                                key={attr.id}
                                className="block w-full text-left px-4 py-2 hover:bg-gray-100"
                                onClick={() => addAttributeToVariant(variantIndex, attr)}
                              >
                                {attr.name}
                              </button>
                            ))}
                            <div className="border-t border-gray-200 px-4 py-2">
                              <input
                                type="text"
                                className="input-field"
                                placeholder="Custom attribute..."
                                onKeyDown={(e) => {
                                  if (e.key === 'Enter' && e.target.value) {
                                    addAttributeToVariant(variantIndex, { name: e.target.value })
                                    e.target.value = ''
                                  }
                                }}
                              />
                            </div>
                          </div>
                        )}
                      </div>
                    </div>
                  </div>
                  
                  <div>
                    <label className="block mb-1 font-medium">
                      Variant Thumbnail
                    </label>
                    <div className="border border-dashed border-gray-300 rounded-lg p-4 text-center">
                      {variant.thumbnailUrl ? (
                        <div className="relative">
                          <img 
                            src={variant.thumbnailUrl || "/placeholder.svg"} 
                            alt="Variant thumbnail" 
                            className="mx-auto h-40 object-contain"
                          />
                          <button 
                            className="absolute top-0 right-0 bg-red-500 text-white rounded-full p-1"
                            onClick={() => {
                              const updatedVariants = [...productData.variants]
                              updatedVariants[variantIndex].thumbnailUrl = null
                              setProductData({...productData, variants: updatedVariants})
                            }}
                          >
                            <X size={16} />
                          </button>
                        </div>
                      ) : (
                        <div 
                          className="flex flex-col items-center justify-center h-40 cursor-pointer"
                          onClick={() => variantFileInputRef.current.click()}
                        >
                          <Upload className="text-gray-400 mb-2" size={24} />
                          <p className="text-sm text-gray-500">Upload Thumbnail</p>
                          <p className="text-xs text-gray-400 mt-1">Click to browse</p>
                          <input
                            type="file"
                            ref={variantFileInputRef}
                            className="hidden"
                            accept="image/*"
                            onChange={(e) => handleVariantThumbnailUpload(e, variantIndex)}
                          />
                        </div>
                      )}
                    </div>
                  </div>
                </div>
              </div>
            ))}
            
            <button 
              className="btn btn-primary flex items-center gap-2"
              onClick={addVariant}
            >
              <Plus size={18} />
              Add Variant
            </button>
          </div>
        )}
        
        {/* Step 4: Images */}
        {currentStep === 4 && (
          <div>
            <h2 className="text-xl font-semibold mb-4">Product Images</h2>
            <p className="text-gray-500 mb-6">Manage additional images for the product</p>
            
            <div className="border border-dashed border-gray-300 rounded-lg p-6 text-center mb-6">
              <div 
                className="flex flex-col items-center justify-center cursor-pointer"
                onClick={() => imagesFileInputRef.current.click()}
              >
                <Upload className="text-gray-400 mb-2" size={32} />
                <p className="text-gray-500">Upload Images</p>
                <p className="text-xs text-gray-400 mt-1">Click to browse or drag and drop</p>
                <p className="text-xs text-gray-400">PNG, JPG, GIF up to 5MB</p>
              </div>
              <input
                type="file"
                ref={imagesFileInputRef}
                className="hidden"
                accept="image/*"
                multiple
                onChange={handleProductImagesUpload}
              />
            </div>
            
            {productData.images.length > 0 && (
              <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4">
                {productData.images.map((image) => (
                  <div key={image.id} className="relative group">
                    <img 
                      src={image.imageUrl || "/placeholder.svg"} 
                      alt={image.name} 
                      className="w-full h-40 object-cover rounded-lg"
                    />
                    <button 
                      className="absolute top-2 right-2 bg-red-500 text-white rounded-full p-1 opacity-0 group-hover:opacity-100 transition-opacity"
                      onClick={() => removeImage(image.id)}
                    >
                      <X size={16} />
                    </button>
                  </div>
                ))}
              </div>
            )}
          </div>
        )}
        
        {/* Navigation Buttons */}
        <div className="flex justify-between mt-8">
          <button 
            className="btn btn-dark flex items-center gap-2"
            onClick={handlePrevious}
            disabled={currentStep === 1}
          >
            <ArrowLeft size={18} />
            Previous
          </button>
          
          <button 
            className="btn btn-primary flex items-center gap-2"
            onClick={handleNext}
          >
            {currentStep === steps.length ? 'Update Product' : 'Next'}
            {currentStep !== steps.length && <ArrowRight size={18} />}
          </button>
        </div>
      </div>
    </div>
  )
}

export default EditProduct