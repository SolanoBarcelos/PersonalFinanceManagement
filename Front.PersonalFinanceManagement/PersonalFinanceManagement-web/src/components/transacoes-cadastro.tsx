import { useState, useEffect } from 'react'
import { 
  TransactionService, 
  PersonService, 
  CategoryService, 
  type PersonResponseDto, 
  type CategoryResponseDto 
} from '../api-client'
import { Button } from './button'
import { Input } from './input'

export function TransacoesCadastro() {
  const [description, setDescription] = useState('')
  const [amount, setAmount] = useState<number | ''>('')
  const [type, setType] = useState<number>(1) // 1: Receita, 2: Despesa
  const [personId, setPersonId] = useState('')
  const [categoryId, setCategoryId] = useState('')
  
  const [persons, setPersons] = useState<PersonResponseDto[]>([])
  const [categories, setCategories] = useState<CategoryResponseDto[]>([])
  const [isLoading, setIsLoading] = useState(false)

  // Carrega Pessoas e Categorias para os Selects
  useEffect(() => {
    const loadData = async () => {
      try {
        const [pRes, cRes] = await Promise.all([
          PersonService.getAll(),
          CategoryService.getAll()
        ])
        setPersons(pRes.data)
        setCategories(cRes.data)
      } catch (err) {
        console.error("Erro ao carregar dependências", err)
      }
    }
    loadData()
  }, [])

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    if (!description || !amount || !personId || !categoryId) {
      alert("Por favor, preencha todos os campos obrigatórios.")
      return
    }

    setIsLoading(true)
    try {
      await TransactionService.create({
        description,
        amount: Number(amount),
        type,
        personId,
        categoryId
      })
      
      alert("Transação registrada com sucesso!")
      
      // Reseta do formulário
      setDescription('')
      setAmount('')
      setPersonId('')
      setCategoryId('')
      setType(1)

    } catch (err: any) {
      // Captura Erros da API
      const errorData = err.response?.data;
      
      const errorMessage = errorData?.Message || 
                           errorData?.message || 
                           "Erro inesperado nas regras de negócio.";

      alert(errorMessage);
      
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <form onSubmit={handleSubmit} className="flex flex-col gap-6 max-w-2xl">
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="flex flex-col gap-2">
          <label className="text-sm font-bold text-primary">Descrição</label>
          <Input 
            value={description} 
            onChange={e => setDescription(e.target.value)} 
            placeholder="Ex: Aluguel, Salário..." 
            required 
          />
        </div>
        <div className="flex flex-col gap-2">
          <label className="text-sm font-bold text-primary">Valor (R$)</label>
          <Input 
            type="number" 
            step="0.01"
            value={amount} 
            onChange={e => setAmount(Number(e.target.value))} 
            placeholder="0,00" 
            required 
          />
        </div>
      </div>

      <div className="flex flex-col gap-2">
        <label className="text-sm font-bold text-primary">Tipo de Transação</label>
        <select 
          className="h-11 rounded-md border border-input bg-surface px-3 text-sm focus:ring-2 focus:ring-primary/20 outline-none"
          value={type} 
          onChange={e => setType(Number(e.target.value))}
        >
          <option value={1}>Receita</option>
          <option value={2}>Despesa</option>
        </select>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="flex flex-col gap-2">
          <label className="text-sm font-bold text-primary">Responsável (Pessoa)</label>
          <select 
            className="h-11 rounded-md border border-input bg-surface px-3 text-sm focus:ring-2 focus:ring-primary/20 outline-none"
            value={personId} 
            onChange={e => setPersonId(e.target.value)} 
            required
          >
            <option value="">Selecione uma pessoa...</option>
            {persons.map(p => (
              <option key={p.id} value={p.id}>
                {p.name} ({p.age} anos)
              </option>
            ))}
          </select>
        </div>
        <div className="flex flex-col gap-2">
          <label className="text-sm font-bold text-primary">Categoria</label>
          <select 
            className="h-11 rounded-md border border-input bg-surface px-3 text-sm focus:ring-2 focus:ring-primary/20 outline-none"
            value={categoryId} 
            onChange={e => setCategoryId(e.target.value)} 
            required
          >
            <option value="">Selecione uma categoria...</option>
            {categories.map(c => (
              <option key={c.id} value={c.id}>
                {c.description}
              </option>
            ))}
          </select>
        </div>
      </div>

      <div className="flex justify-end mt-4">
       <Button 
          type="submit" 
          variant="primary" 
          size="lg" 
          disabled={isLoading}
          className="bg-primary hover:bg-primary/90 text-white px-8 py-2 rounded-md transition-all"
        >
          {isLoading ? 'Salvando...' : 'Confirmar'}
        </Button>
      </div>
    </form>
  )
}