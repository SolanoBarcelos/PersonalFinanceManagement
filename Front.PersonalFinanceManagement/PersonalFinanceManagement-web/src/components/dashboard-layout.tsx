import { useState } from 'react'
import { Tabs, TabsList, TabsTab, TabsPanel } from './tabs'
import { PessoasCadastro } from './pessoas-cadastro'
import { PessoasListagem } from './pessoas-listagem'
import { CategoriasCadastro } from './categorias-cadastro'
import { CategoriasListagem } from './categorias-listagem'
import { TransacoesCadastro } from './transacoes-cadastro'
import { TransacoesListagem } from './transacoes-listagem'
import { RelatoriosPainel } from './relatorios-painel'

export function DashboardLayout() {
  const [mainTab, setMainTab] = useState('cadastros')
  const [subTab, setSubTab] = useState('pessoas')

  const handleMainTabChange = (value: string) => {
    setMainTab(value)
    setSubTab('pessoas') 
  }

  return (
    <div className="min-h-screen bg-surface-raised pt-8 px-20 flex flex-col items-center">
      {/* gap-20 cria o afastamento de 2cm entre header/menu e o conteúdo */}
      <div className="w-full max-w-7xl flex flex-col gap-20">
        
        <header>
          <h1 className="text-3xl font-bold text-primary">Gestão de Finanças Pessoais</h1>
        </header>

        <Tabs className="w-full">
          {/* NÍVEL 1: Módulos Principais */}
          <TabsList>
            <TabsTab 
              value="cadastros" 
              selectedValue={mainTab} 
              onClick={() => handleMainTabChange('cadastros')}
            >
              Cadastros
            </TabsTab>
            <TabsTab 
              value="listagem" 
              selectedValue={mainTab} 
              onClick={() => handleMainTabChange('listagem')}
            >
              Listagem
            </TabsTab>
            <TabsTab 
              value="relatorios" 
              selectedValue={mainTab} 
              onClick={() => handleMainTabChange('relatorios')}
            >
              Relatórios
            </TabsTab>
          </TabsList>

          {/* NÍVEL 2: Conteúdo com distância em relação ao Nível 1 */}
          <div className="w-full mt-10"> {/* Distância adicional */}
            
            {/* RENDERIZAÇÃO CONDICIONAL BASEADA NO MAIN TAB */}
            {mainTab === 'cadastros' && (
              <Tabs className="w-full">
                <TabsList>
                  <TabsTab value="pessoas" selectedValue={subTab} onClick={() => setSubTab('pessoas')}>Pessoas</TabsTab>
                  <TabsTab value="categorias" selectedValue={subTab} onClick={() => setSubTab('categorias')}>Categorias</TabsTab>
                  <TabsTab value="transacoes" selectedValue={subTab} onClick={() => setSubTab('transacoes')}>Transações</TabsTab>
                </TabsList>
                <TabsPanel value="pessoas" selectedValue={subTab}><PessoasCadastro /></TabsPanel>
                <TabsPanel value="categorias" selectedValue={subTab}><CategoriasCadastro /></TabsPanel>
                <TabsPanel value="transacoes" selectedValue={subTab}><TransacoesCadastro /></TabsPanel>
              </Tabs>
            )}

            {mainTab === 'listagem' && (
              <Tabs className="w-full">
                <TabsList>
                  <TabsTab value="pessoas" selectedValue={subTab} onClick={() => setSubTab('pessoas')}>Pessoas</TabsTab>
                  <TabsTab value="categorias" selectedValue={subTab} onClick={() => setSubTab('categorias')}>Categorias</TabsTab>
                  <TabsTab value="transacoes" selectedValue={subTab} onClick={() => setSubTab('transacoes')}>Transações</TabsTab>
                </TabsList>
                <TabsPanel value="pessoas" selectedValue={subTab}><PessoasListagem /></TabsPanel>
                <TabsPanel value="categorias" selectedValue={subTab}><CategoriasListagem /></TabsPanel>
                <TabsPanel value="transacoes" selectedValue={subTab}><TransacoesListagem /></TabsPanel>
              </Tabs>
            )}

            {mainTab === 'relatorios' && (
              <Tabs className="w-full">
                <TabsList>
                  <TabsTab value="pessoas" selectedValue={subTab} onClick={() => setSubTab('pessoas')}>Saldos por Pessoa</TabsTab>
                  <TabsTab value="categorias" selectedValue={subTab} onClick={() => setSubTab('categorias')}>Saldos por Categoria</TabsTab>
                </TabsList>
                <TabsPanel value="pessoas" selectedValue={subTab}><RelatoriosPainel type="persons" /></TabsPanel>
                <TabsPanel value="categorias" selectedValue={subTab}><RelatoriosPainel type="categories" /></TabsPanel>
              </Tabs>
            )}
          </div>
        </Tabs>
      </div>
    </div>
  )
}