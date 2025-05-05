import { supabaseClient } from './supabaseClient'

/**
 * Generate unique file name
 */
const generateUniqueFileName = (file) => {
  const fileExt = file.name.split('.').pop()
  const fileName = `${Date.now()}-${Math.random().toString(36).substring(2, 15)}.${fileExt}`
  return fileName
}

/**
 * Upload 1 file to Supabase & return public URL + storage path
 */
export const uploadSingleFile = async (bucket, folderPath, file) => {
  const fileName = generateUniqueFileName(file)
  const filePath = `${folderPath}/${fileName}`

  const { error: uploadError } = await supabaseClient
    .storage
    .from(bucket)
    .upload(filePath, file, {
      cacheControl: '3600',
      upsert: false
    })

  if (uploadError) throw uploadError

  const { data: { publicUrl } } = supabaseClient
    .storage
    .from(bucket)
    .getPublicUrl(filePath)

  return { publicUrl, filePath }
}

/**
 * Upload multiple file
 */
export const uploadMultipleFiles = async (bucket, folderPath, files) => {
  const uploadPromises = files.map(async (file) => {
    const { publicUrl, filePath } = await uploadSingleFile(bucket, folderPath, file)
    return {
      id: `${Date.now()}-${Math.random().toString(36).substr(2, 9)}`,
      name: file.name,
      imageUrl: publicUrl,
      storagePath: filePath
    }
  })

  return await Promise.all(uploadPromises)
}

/**
 * remove image from Supabase storage
 */
export const removeFileFromStorage = async (bucket, filePath) => {
  const { error } = await supabaseClient
    .storage
    .from(bucket)
    .remove([filePath])

  if (error) throw error
}
