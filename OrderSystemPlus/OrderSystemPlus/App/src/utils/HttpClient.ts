import axios from 'axios'
import type { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios'
import MessageBox from '@/utils/MessageBox.ts'

class HttpClient {
  private axiosInstance: AxiosInstance
  private axioswithCredentialsInstance: AxiosInstance
  private baseURL: string
  constructor(baseURL: string) {
    this.baseURL = baseURL
    this.axiosInstance = axios.create({
      baseURL: this.baseURL,
      timeout: 600000,
    })
    this.axioswithCredentialsInstance = axios.create({
      baseURL: this.baseURL,
      timeout: 600000,
      withCredentials: true
    })
  }

  public getConfig = (): AxiosRequestConfig => {
    const config = {} as AxiosRequestConfig
    config.headers = {'Access-Control-Allow-Origin': '*',mode: 'no-cors'}
    
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
      if (error?.response?.status === 401) {
        // Auto Reresh Token
        try {
          const rsp: AxiosResponse<T> = await this.axioswithCredentialsInstance.post(
            import.meta.env.VITE_APP_AXIOS_AUTH_REFRESHACCESSTOKEN,
            data
          )
          localStorage.setItem('accessToken', rsp?.data?.accessToken)

          const config = this.getConfig()
          const response: AxiosResponse<T> = await this.axiosInstance.post(url, data, config)
          return response.data
        } catch (refreshError) {
          console.error('Refresh token error.Direct to SignIn.')
        }
      } else if (error?.response?.status === 500) {
        var message = error?.response?.data?.message
        if (message != '') {
          MessageBox.showErrorMessage(message)
        } else {
          MessageBox.showErrorMessage('Internal Server Error')
        }
        throw error
      } else {
        throw error
      }
    }
  }
  public async postWithCredentials<T>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig
  ): Promise<T> {
    try {
      const config = this.getConfig()
      const response: AxiosResponse<T> = await this.axioswithCredentialsInstance.post(
        url,
        data,
        config
      )
      return response.data
    } catch (error) {
      if (error?.response?.status === 401) {
        // Auto Reresh Token
        try {
          const rsp: AxiosResponse<T> = await this.axioswithCredentialsInstance.post(
            import.meta.env.VITE_APP_AXIOS_AUTH_REFRESHACCESSTOKEN,
            data
          )
          localStorage.setItem('accessToken', rsp?.data?.accessToken)

          const config = this.getConfig()
          const response: AxiosResponse<T> = await this.axioswithCredentialsInstance.post(
            url,
            data,
            config
          )
          return response.data
        } catch (refreshError) {
          console.error('Refresh token error.Direct to SignIn.')
        }
      } else if (error?.response?.status === 500) {
        var message = error?.response?.data?.message
        if (message != '') {
          MessageBox.showErrorMessage(message)
        } else {
          MessageBox.showErrorMessage('Internal Server Error')
        }
        throw error
      } else {
        throw error
      }
    }
  }
}

const httpClient = new HttpClient(import.meta.env.VITE_APP_AXIOS_BASE_URL)
export default httpClient
