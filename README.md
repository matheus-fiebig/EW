### Visão geral:

O time de desenvolvimento de uma empresa precisa de sua ajuda para criar um sistema de gerenciamento de tarefas. O objetivo é desenvolver uma **API** que permita aos usuários organizar e monitorar suas tarefas diárias, bem como colaborar com colegas de equipe.

---

### Detalhes do App:

**Usuário**

Pessoa que utiliza o aplicativo detentor de uma conta.

**Projeto**

Um projeto é uma entidade que contém várias tarefas. Um usuário pode criar, visualizar e gerenciar vários projetos.

**Tarefa**

Uma tarefa é uma unidade de trabalho dentro de um projeto. Cada tarefa possui um título, uma descrição, uma data de vencimento e um status (pendente, em andamento, concluída).

---

### Fase 1: API Coding

Para a primeira Sprint, foi estipulado o desenvolvimento de funcionalidades básicas para o gerenciamento de tarefas. Desenvolva uma RESTful API capaz de responder a requisições feitas pelo aplicativo para os seguintes itens:

1. **Listagem de Projetos** - listar todos os projetos do usuário
2. **Visualização de Tarefas** - visualizar todas as tarefas de um projeto específico
3. **Criação de Projetos** - criar um novo projeto
4. **Criação de Tarefas** - adicionar uma nova tarefa a um projeto
5. **Atualização de Tarefas** - atualizar o status ou detalhes de uma tarefa
6. **Remoção de Tarefas** - remover uma tarefa de um projeto

**Regras de negócio:**

1. **Prioridades de Tarefas:**
    - Cada tarefa deve ter uma prioridade atribuída (baixa, média, alta).
    - Não é permitido alterar a prioridade de uma tarefa depois que ela foi criada.
2. **Restrições de Remoção de Projetos:**
    - Um projeto não pode ser removido se ainda houver tarefas pendentes associadas a ele.
    - Caso o usuário tente remover um projeto com tarefas pendentes, a API deve retornar um erro e sugerir a conclusão ou remoção das tarefas primeiro.
3. **Histórico de Atualizações:**
    - Cada vez que uma tarefa for atualizada (status, detalhes, etc.), a API deve registrar um histórico de alterações para a tarefa.
    - O histórico de alterações deve incluir informações sobre o que foi modificado, a data da modificação e o usuário que fez a modificação.
4. **Limite de Tarefas por Projeto:**
    - Cada projeto tem um limite máximo de 20 tarefas. Tentar adicionar mais tarefas do que o limite deve resultar em um erro.
5. **Relatórios de Desempenho:**
    - A API deve fornecer endpoints para gerar relatórios de desempenho, como o número médio de tarefas concluídas por usuário nos últimos 30 dias.
    - Os relatórios devem ser acessíveis apenas por usuários com uma função específica de "gerente".
6. **Comentários nas Tarefas:**
    - Os usuários podem adicionar comentários a uma tarefa para fornecer informações adicionais.
    - Os comentários devem ser registrados no histórico de alterações da tarefa.

**Regras da API e avaliação:**

1. **Não é** **necessário** nenhum tipo de CRUD para usuários.
2. **Não é necessário** nenhum tipo de autenticação; este será um serviço externo.
3. Tenha pelo menos **80%** de cobertura de testes de unidade para validar suas regras de negócio.
4. **Utilize o git** como ferramenta de versionamento de código.
5. **Utilize um banco de dados** (o que preferir) para salvar os dados.
6. **Utilize o framework e libs** que julgue necessário para uma boa implementação.
7. **O projeto deve executar no docker e as informações de execução via terminal devem estar disponíveis no [README.md](http://README.md) do projeto**

---

### Fase 2: Refinamento

Para a segunda fase, escreva no arquivo **README.md** em uma sessão dedicada, o que você perguntaria para o *PO* visando o refinamento para futuras implementações ou melhorias.

---

### Fase 3: Final

Na terceira fase, escreva no arquivo **README.md** em uma sessão dedicada o que você melhoraria no projeto, identificando possíveis pontos de melhoria, implementação de padrões, visão do projeto sobre arquitetura/cloud, etc.

---

# Como Rodar
Execute o comando:  
- `docker compose up`  
- **OBS**: Caso enfrente problemas ao tentar realizar o pull das imagens, acesse a pasta do usuário `\.docker` e exclua o arquivo `config`. **Faça um backup antes para evitar problemas**.  

---

# Explicação Breve e Tecnologias  
- Neste projeto, optei por utilizar o **Mediatr** para a criação dos casos de uso. Dessa forma, cada caso de uso possui seu próprio handler. Acredito que isso proporciona maior organização, flexibilidade para criação de eventos e maior objetividade no código.  
- Além disso, estou utilizando o conceito de **domínio rico**, ou seja, toda a regra de negócio está encapsulada dentro da entidade correspondente.  
- Apesar de não utilizar dois bancos de dados distintos, o projeto segue organizado com base no padrão **CQRS**.  
- O banco de dados escolhido foi o **SQL Server**, utilizando o **Entity Framework** para a persistência.  

---

# Refinamento  
- **Por que um projeto só pode ter 20 tarefas?**  
  Isso limita o planejamento antecipado de POs/PMs, entre outros. Talvez um limite configurável durante a criação do projeto seja mais eficaz. Outra opção seria limitar o número de tarefas por progresso, como, por exemplo, um máximo de 10 tarefas no status "Fazendo".  

- **Restrições para alterar a prioridade das tarefas após a criação:**  
  Essa restrição pode impactar negativamente ambientes altamente dinâmicos. Uma solução seria tornar essa regra configurável ou definir condições específicas, como restringir a alteração apenas em determinados status, mas não em todos.  

- **Deleção de projetos/tarefas:**  
  A deleção do dado será física ou lógica (remover o registro ou so esconde-lo)?  

- **Histórico de alterações:**  
  O histórico será utilizado apenas para auditoria ou haverá alguma regra de negócio associada a ele no futuro?  

- **Relatórios e dados importantes:**  
  Quais dados são essenciais para o negócio? Os relatórios serão específicos para cada situação ou haverá um **dashboard** que permita filtragem? A solicitação de relatórios está muito genérica: não define quantos relatórios serão gerados nem o formato (Excel, PDF, dashboard).  

- **Validações de entrada e erros:**  
  É importante solicitar ao PO uma lista de erros possíveis (validações de entrada e regras de negócio, como campos nulos, limites de caracteres, etc.) para melhorar o retorno ao usuário. Caso o PO não forneça essa lista, eu realizaria as validações e encaminharia ao PO uma lista dos possíveis erros, para que ele e o time de UI/UX validem as mensagens.  

- **Serviço de autenticação externo:**  
  O serviço de autenticação retorna o ID e o perfil do usuário, correto? Precisamos validar como esses dados serão mapeados no nosso sistema (necessidade de um de-para).  

- **Mensagens de erro:**  
  Será exibida apenas uma mensagem de erro por vez ou todas de uma vez?  

---

# Final  
- **Agregador de mensagens de erro:**  
  Pode ser necessário criar um agregador para facilitar o retorno de erros ao usuário. Uma solução seria implementar uma classe de validador com dois modos: agregador e único, dependendo da necessidade. Sugiro um padrão semelhante ao do FluentValidation (mas sem utilizá-lo diretamente). Um exemplo pode ser encontrado [neste código](https://github.com/matheus-fiebig/functional-validation-poc/blob/main/Application/Customers/Services/CustomerService.cs), no método `CreateCustomer`.  

- **Logs:**  
  Embora este projeto ainda não contenha logs, é essencial incluí-los antes do GoLive.  

- **Soft delete vs. hard delete:**  
  Considerar soft delete pode ser necessário, dependendo das decisões do PO.  

- **Divisão de contextos (Query e Command):**  
  Pode ser útil caso o projeto cresça e seja necessário utilizar dois bancos (um para leitura e outro para escrita). Entretanto, neste momento inicial, essa abordagem pode gerar custos desnecessários e não ser prioritária para um MVP.  

- **Otimizações de performance:**  
  Utilizar `AsNoTracking()` e configurar seu uso adequadamente para otimizar a performance.  

- **Configurações sensíveis:**  
  Recomenda-se utilizar um **Key Vault** ou variáveis de ambiente para armazenar configurações do `appsettings`.  

- **Microserviços e histórico:**  
  Caso seja um microserviço, pode ser interessante criar uma **Minimal API** ou um **worker** exclusivo para salvar o histórico. Porém, isso implica em custos adicionais e deve ser avaliado se realmente é necessário.  

- **Escalabilidade:**  
  Para melhorar a escalabilidade, pode ser interessante organizar os projetos por feature. Por exemplo, ao invés de utilizar uma estrutura como `EclipseWorks` e `EclipseWorks.Application`, seria mais eficiente algo como `Task.EclipseWorks` e `Task.EclipseWorks.Application`. Assim, no futuro, seria mais fácil separar partes do projeto sem causar grandes impactos.
