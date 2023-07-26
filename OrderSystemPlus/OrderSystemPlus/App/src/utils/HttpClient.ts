import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios'
import MessageBox from '@/utils/MessageBox.ts'

class HttpClient {
  private axiosInstance: AxiosInstance

  constructor(baseURL: string) {
    this.axiosInstance = axios.create({
      baseURL: baseURL,
      timeout: 10000,
      withCredentials: true
    })
  }

  public getConfig = (): AxiosRequestConfig => {
    const config = {} as AxiosRequestConfig
    config.headers = {}
    const bearerToken = localStorage.getItem('accessToken')
    if (bearerToken) config.headers['Authorization'] = `Bearer ${bearerToken}`
    return config
  }

  public async get<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
    const response: AxiosResponse<T> = await this.axiosInstance.get(url, config)
    return response.data
  }

  public async post<T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> {
    try {
      const config = this.getConfig()
      const response: AxiosResponse<T> = await this.axiosInstance.post(url, data, config)
      return response.data
    } catch (error) {
      if (error?.response?.status === 500) {
        var message = error?.response?.data?.message
        if (message != '') {
          MessageBox.showErrorMessage(message)
        } else {
          MessageBox.showErrorMessage('Internal Server Error')
        }
      }
      throw error
    }
  }
}

const httpClient = new HttpClient(import.meta.env.VITE_APP_AXIOS_BASE_URL)
export default httpClient
