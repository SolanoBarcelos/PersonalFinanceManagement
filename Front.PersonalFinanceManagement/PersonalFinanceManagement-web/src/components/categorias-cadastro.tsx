import { useState } from 'react'
import { CategoryService } from '../api-client'
import { Button } from './button'
import { Input } from './input'

export function CategoriasCadastro() {
  const [description, setDescription] = useState('')
  const [isReceita, setIsReceita] = useState(false)
  const [isDespesa, setIsDespesa] = useState(false)
  const [isLoading, setIsLoading] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    // Mapeamento do Enum esperado para back-end
    let purpose = 0;
    if (isReceita && !isDespesa) purpose = 1;      // Receita
    else if (!isReceita && isDespesa) purpose = 2; // Despesa
    else if (isReceita && isDespesa) purpose = 3;  // Ambas

    setIsLoading(true)
    
    try {
      await CategoryService.create({ 
        description: description.trim(), 
        purpose 
      })
      
      alert('Categoria registrada com sucesso!')
      
      // Limpa tela apos cadastrar
      setDescription('')
      setIsReceita(false)
      setIsDespesa(false)
    } catch (error: any) {

      // Acessa a propriedade 'Message' do DTO de erro
      const errorData = error.response?.data;
      
      const errorMessage = errorData?.Message || 
                           errorData?.message || 
                           "Erro ao registrar categoria. Verifique as regras de negócio.";

      alert(errorMessage); // Exibe a mensagem retornada pela API
      
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <form
      data-slot="form-categorias"
      onSubmit={handleSubmit}
      className="flex flex-col gap-6 max-w-2xl"
    >
      <div className="flex flex-col gap-2">
        <label htmlFor="description" className="text-sm font-semibold text-foreground">
          Descrição:
        </label>
        <Input
          id="description"
          placeholder="Insira descrição aqui"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          disabled={isLoading}
          maxLength={400}
          required
        />
      </div>

      <div className="flex flex-col gap-3">
        <label className="text-sm font-semibold text-foreground">
          Finalidade:
        </label>
        <div className="flex flex-col gap-2 p-4 border border-input rounded-md bg-surface">
          <p className="text-xs text-muted-foreground mb-2">Selecione os tipos de finalidade</p>
          
          <label className="flex items-center gap-2 cursor-pointer">
            <input 
              type="checkbox" 
              checked={isDespesa}
              onChange={(e) => setIsDespesa(e.target.checked)}
              disabled={isLoading}
              className="size-4 accent-primary"
            />
            <span className="text-sm text-foreground">Despesa</span>
          </label>

          <label className="flex items-center gap-2 cursor-pointer">
            <input 
              type="checkbox" 
              checked={isReceita}
              onChange={(e) => setIsReceita(e.target.checked)}
              disabled={isLoading}
              className="size-4 accent-primary"
            />
            <span className="text-sm text-foreground">Receita</span>
          </label>
        </div>
      </div>

      <div className="flex justify-end pt-4">
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