import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios'

class HttpClient {
  private axiosInstance: AxiosInstance

  constructor(baseURL: string) {
    this.axiosInstance = axios.create({
      baseURL: baseURL,
      timeout: 10000
    })
  }

  public async get<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
    const response: AxiosResponse<T> = await this.axiosInstance.get(url, config)
    return response.data
  }

  public async post<T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> {
    const response: AxiosResponse<T> = await this.axiosInstance.post(url, data, config)
    return response.data
  }
}

const httpClient = new HttpClient(import.meta.env.VITE_APP_AXIOS_BASE_URL)
export default httpClient
