import axios from 'axios'

// Lógica de ambiente: Busca a URL do .env, se não achar, usa a local do Swagger
const baseURL = import.meta.env.VITE_API_URL || 'https://localhost:7281/api'

export const api = axios.create({
  baseURL,
  headers: {
    'Content-Type': 'application/json',
  },
})

// DTOs (Data Transfer Objects) - Contratos

// Pessoas
export interface PersonResponseDto {
  id: string
  name: string
  age: number
}

export interface CreatePersonDto {
  name: string
  age: number
}

export interface UpdatePersonDto {
  id: string
  name: string
  age: number
}

// Categorias
export interface CategoryResponseDto {
  id: string
  description: string
  purpose: number 
}

export interface CreateCategoryDto {
  description: string
  purpose: number
}

// Transações
export interface CreateTransactionDto {
  description: string
  amount: number
  type: number // 1: Receita, 2: Despesa
  categoryId: string
  personId: string
}

export interface TransactionResponseDto {
  id: string
  description: string
  amount: number
  type: number
  personId: string;    
  categoryId: string;  
}

// Relatórios
export interface ReportItemDto {
  nameOrDescription: string
  totalIncome: number
  totalExpense: number
  balance: number
}

export interface ReportResultDto {
  items: ReportItemDto[]
  grandTotalIncome: number
  grandTotalExpense: number
  grandTotalBalance: number
}

// Serviços (Encapsulamento de Padrão Repository)
export const PersonService = {
  getAll: () => api.get<PersonResponseDto[]>('/Person'),
  getById: (id: string) => api.get<PersonResponseDto>(`/Person/${id}`),
  create: (data: CreatePersonDto) => api.post('/Person', data),
  update: (id: string, data: UpdatePersonDto) => api.put(`/Person/${id}`, data),
  delete: (id: string) => api.delete(`/Person/${id}`),
}

export const CategoryService = {
  getAll: () => api.get<CategoryResponseDto[]>('/Category'),
  create: (data: CreateCategoryDto) => api.post('/Category', data),
}

export const TransactionService = {
  create: (data: CreateTransactionDto) => api.post('/Transaction', data),
  getAll: () => api.get<TransactionResponseDto[]>('/Transaction'),
}

export const ReportService = {
  getPersonsReport: () => api.get<ReportResultDto>('/Report/persons'),
  getCategoriesReport: () => api.get<ReportResultDto>('/Report/categories'),
}