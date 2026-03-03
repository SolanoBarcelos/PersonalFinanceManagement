import { useEffect, useState } from 'react'
import { CategoryService, type CategoryResponseDto } from '../api-client'
import { Button } from './button'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from './table'

export function CategoriasListagem() {
  const [categories, setCategories] = useState<CategoryResponseDto[]>([])
  const [isLoading, setIsLoading] = useState(true)

  const fetchCategories = async () => {
    setIsLoading(true)
    try {
      const response = await CategoryService.getAll()
      setCategories(response.data)
    } catch (error) {
      console.error('Erro ao buscar categorias:', error)
      alert('Não foi possível carregar a lista de categorias.')
    } finally {
      setIsLoading(false)
    }
  }

  useEffect(() => {
    fetchCategories()
  }, [])

  // Função pura para traduzir o Enum do Domínio para Texto da interface gráfica
  const getPurposeText = (purpose: number) => {
    switch (purpose) {
      case 1: return 'Receita'
      case 2: return 'Despesa'
      case 3: return 'Receita e Despesa'
      default: return 'Indefinido'
    }
  }

  // Função pura para definir a cor da tag (Badge) baseada no propósito
  const getPurposeBadgeColor = (purpose: number) => {
    switch (purpose) {
      case 1: return 'bg-success/20 text-success border-success/30'
      case 2: return 'bg-destructive/20 text-destructive border-destructive/30'
      case 3: return 'bg-primary/20 text-primary border-primary/30'
      default: return 'bg-muted text-muted-foreground'
    }
  }

  return (
    <div className="flex flex-col gap-6">
      
      {/* Barra de Ferramentas Superior - Apenas o botão recarregar alinhado à direita */}
      <div className="flex justify-end items-end gap-4">
        <Button onClick={fetchCategories} variant="secondary" className="whitespace-nowrap">
          Recarregar Dados
        </Button>
      </div>

      {/* Tabela de Dados */}
      {isLoading ? (
        <div className="flex justify-center items-center h-32 border border-border rounded-lg bg-surface">
          <p className="text-muted-foreground animate-pulse">Carregando categorias...</p>
        </div>
      ) : (
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead className="w-1/2">DESCRIÇÃO</TableHead>
              <TableHead className="w-1/4">FINALIDADE</TableHead>
              <TableHead className="w-1/4">ID DA CATEGORIA</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {categories.length === 0 ? (
              <TableRow>
                <TableCell colSpan={3} className="text-center text-muted-foreground py-8">
                  Nenhuma categoria encontrada.
                </TableCell>
              </TableRow>
            ) : (
              categories.map((category) => (
                <TableRow key={category.id}>
                  <TableCell className="font-medium text-foreground">
                    {category.description}
                  </TableCell>
                  <TableCell>
                    {/* Badge visual para a finalidade */}
                    <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium border ${getPurposeBadgeColor(category.purpose)}`}>
                      {getPurposeText(category.purpose)}
                    </span>
                  </TableCell>
                  <TableCell className="font-mono text-xs text-muted-foreground">
                    {category.id}
                  </TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      )}
    </div>
  )
}