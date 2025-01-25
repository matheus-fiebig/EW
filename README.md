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
