import { useState } from 'react'
import { PersonService } from '../api-client'
import { Button } from './button'
import { Input } from './input'

export function PessoasCadastro() {
  const [name, setName] = useState('')
  const [age, setAge] = useState<number | ''>('')
  const [isLoading, setIsLoading] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    setIsLoading(true)
    
    try {
      // Chamada para a API
      await PersonService.create({ 
        name: name.trim(), 
        age: Number(age) 
      })
      
      alert('Pessoa cadastrada com sucesso!')
      
      // Limpa o formulário após cadastro
      setName('')
      setAge('')
    } catch (error: any) {

      // Acessa a propriedade 'Message' de Erro
      const errorData = error.response?.data;
      
      const errorMessage = errorData?.Message || 
                           errorData?.message || 
                           "Erro ao realizar o cadastro. Verifique se o servidor está ativo.";

      alert(errorMessage); // Exibe a mensagem do Backend
      
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <form
      data-slot="form-pessoas"
      onSubmit={handleSubmit}
      className="flex flex-col gap-6 max-w-2xl"
    >
      <div className="flex flex-col gap-2">
        <label htmlFor="name" className="text-sm font-semibold text-foreground">
          Nome:
        </label>
        <Input
          id="name"
          placeholder="Insira nome aqui"
          value={name}
          onChange={(e) => setName(e.target.value)}
          disabled={isLoading}
          maxLength={400}
          required // Evita envio nulo acidental
        />
      </div>

      <div className="flex flex-col gap-2">
        <label htmlFor="age" className="text-sm font-semibold text-foreground">
          Idade:
        </label>
        <Input
          id="age"
          type="number"
          placeholder="Insira idade aqui"
          value={age}
          onChange={(e) => setAge(e.target.value === '' ? '' : Number(e.target.value))}
          disabled={isLoading}
          required // "Erro" para tentativa de envio nulo
        />
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