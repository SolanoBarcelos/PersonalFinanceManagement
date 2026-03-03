import { useEffect, useState } from 'react'
import { TransactionService, type TransactionResponseDto } from '../api-client'
import { Button } from './button'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from './table'
import { twMerge } from 'tailwind-merge'


export function TransacoesListagem() {

  const [transactions, setTransactions] = useState<TransactionResponseDto[]>([])

  const [isLoading, setIsLoading] = useState(true)

  const fetchTransactions = async () => {

    setIsLoading(true)

    try {

      const response = await TransactionService.getAll()

      setTransactions(response.data)

    } catch (error) {

      console.error('Erro ao buscar transações:', error)

      alert('Não foi possível carregar a lista de transações.')

    } finally {

      setIsLoading(false)

    }

  }

  useEffect(() => {

    fetchTransactions()

  }, [])

  return (

    <div className="flex flex-col gap-6">
      <div className="flex justify-between items-center">
        <h2 className="text-lg font-bold text-primary"></h2>
        <Button
          onClick={fetchTransactions}
          className="bg-[#2b44a1] text-white hover:bg-[#1e327a] px-4 py-2 rounded-md font-medium">
          Recarregar Dados
        </Button>
      </div>
      {isLoading ? (
        <div className="flex justify-center items-center h-32 border border-border rounded-lg bg-surface">
          <p className="text-muted-foreground animate-pulse">Carregando histórico...</p>
        </div>        
      ) : (
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>DESCRIÇÃO</TableHead>
              <TableHead>ID PESSOA</TableHead>
              <TableHead>ID CATEGORIA</TableHead>
              <TableHead className="text-center">TIPO</TableHead>
              <TableHead className="text-right">VALOR</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {transactions.length === 0 ? (
              <TableRow>
                <TableCell colSpan={5} className="text-center text-muted-foreground py-8">
                  Nenhuma transação registrada.
                </TableCell>
              </TableRow>
            ) : (                
              transactions.map((t) => (
                <TableRow key={t.id}>
                  <TableCell className="font-medium">{t.description}</TableCell>       

                  {/* Exibicão dos Ids de Pessoas e Categorias*/}
                  <TableCell className="font-mono text-[10px] text-muted-foreground break-all max-w-[150px]">
                    {t.personId}
                  </TableCell>
                  <TableCell className="font-mono text-[10px] text-muted-foreground break-all max-w-[150px]">
                    {t.categoryId}
                  </TableCell>
                  <TableCell className="text-center">
                    <span className={twMerge(
                      'px-2 py-1 rounded text-[10px] font-bold uppercase',
                      t.type === 1 ? 'bg-success/10 text-success' : 'bg-destructive/10 text-destructive'
                    )}>
                      {t.type === 1 ? 'Receita' : 'Despesa'}
                    </span>
                  </TableCell>
                  <TableCell className={twMerge(
                    'text-right font-bold',
                    t.type === 1 ? 'text-success' : 'text-destructive'
                  )}>
                    {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(t.amount)}
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