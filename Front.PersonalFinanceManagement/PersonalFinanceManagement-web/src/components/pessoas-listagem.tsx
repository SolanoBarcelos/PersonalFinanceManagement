import { useEffect, useState } from 'react'
import { PersonService, type PersonResponseDto } from '../api-client'
import { Button } from './button'
import { Input } from './input'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from './table'

export function PessoasListagem() {
  const [persons, setPersons] = useState<PersonResponseDto[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [searchId, setSearchId] = useState('')

  const fetchPersons = async () => {
    setIsLoading(true)
    try {
      const response = await PersonService.getAll()
      setPersons(response.data)
      setSearchId('')
    } catch (error) {
      console.error('Erro ao buscar pessoas:', error)
      alert('Não foi possível carregar a lista de pessoas.')
    } finally {
      setIsLoading(false)
    }
  }

  useEffect(() => {
    fetchPersons()
  }, [])

  const handleSearchById = async () => {
    if (!searchId.trim()) {
      alert('Por favor, insira um ID válido.')
      return
    }

    setIsLoading(true)
    try {
      const response = await PersonService.getById(searchId.trim())
      setPersons([response.data]) 
    } catch (error: any) {
      if (error.response?.status === 404) {
        alert('Pessoa não encontrada com este ID.')
        setPersons([])
      } else {
        console.error('Erro ao buscar pessoa:', error)
        alert('Erro ao realizar a busca.')
      }
    } finally {
      setIsLoading(false)
    }
  }

  const handleDelete = async (id: string) => {
    const confirmDelete = window.confirm('Tem certeza que deseja excluir este registro?')
    if (!confirmDelete) return

    try {
      await PersonService.delete(id)
      alert('Pessoa removida com sucesso!')
      setPersons(prev => prev.filter(p => p.id !== id))
    } catch (error) {
      console.error('Erro ao deletar:', error)
      alert('Erro ao tentar excluir a pessoa.')
    }
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-4 gap-8">
      
      <div className="flex flex-col gap-6 md:col-span-1 border-r border-border pr-6">
        <div className="flex flex-col gap-2">
          <label className="text-sm font-bold text-primary">Buscar por ID</label>
          <Input 
            placeholder="Insira o ID da pessoa" 
            value={searchId}
            onChange={(e) => setSearchId(e.target.value)}
            className="border-border rounded-md outline-none focus-visible:ring-primary/20"
          />
        </div>
        
        <Button 
          onClick={handleSearchById} 
          className="w-full !bg-[#2b44a1] !text-white hover:!bg-[#1e327a] rounded-md font-medium px-4 py-2"
        >
          Confirmar
        </Button>

        <div className="mt-8">
          <Button 
            onClick={fetchPersons} 
            className="w-full !bg-[#2b44a1] !text-white hover:!bg-[#1e327a] rounded-md font-medium px-4 py-2"
          >
            Buscar todos
          </Button>
        </div>
      </div>

      <div className="md:col-span-3">
        {isLoading ? (
          <div className="flex justify-center items-center h-32 border border-border rounded-lg bg-surface">
            <p className="text-muted-foreground animate-pulse">Carregando dados...</p>
          </div>
        ) : (
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>NOME</TableHead>
                <TableHead>ID</TableHead>
                <TableHead className="text-center">IDADE</TableHead>
                <TableHead className="text-right"></TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {persons.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={4} className="text-center text-muted-foreground py-8">
                    Nenhum registro encontrado.
                  </TableCell>
                </TableRow>
              ) : (
                persons.map((person) => (
                  <TableRow key={person.id}>
                    <TableCell className="font-medium">
                      {person.name}
                    </TableCell>
                    <TableCell className="font-mono text-[10px] text-muted-foreground break-all max-w-[150px]">
                      {person.id}
                    </TableCell>
                    <TableCell className="text-center">{person.age}</TableCell>
                    <TableCell className="text-right">
                      <Button 
                        onClick={() => handleDelete(person.id)} 
                        className="!bg-destructive !text-white hover:!bg-destructive/90 rounded-md px-4 py-1 text-xs font-medium"
                      >
                        Deletar
                      </Button>
                    </TableCell>
                  </TableRow>
                ))
              )}
            </TableBody>
          </Table>
        )}
      </div>
    </div>
  )
}