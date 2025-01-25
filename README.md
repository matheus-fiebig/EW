# Como Rodar
Execute o comando:  
- `docker compose up`  
- **OBS**: Caso enfrente problemas ao tentar realizar o pull das imagens, acesse a pasta do usu�rio `\.docker` e exclua o arquivo `config`. **Fa�a um backup antes para evitar problemas**.  

---

# Explica��o Breve e Tecnologias  
- Neste projeto, optei por utilizar o **Mediatr** para a cria��o dos casos de uso. Dessa forma, cada caso de uso possui seu pr�prio handler. Acredito que isso proporciona maior organiza��o, flexibilidade para cria��o de eventos e maior objetividade no c�digo.  
- Al�m disso, estou utilizando o conceito de **dom�nio rico**, ou seja, toda a regra de neg�cio est� encapsulada dentro da entidade correspondente.  
- Apesar de n�o utilizar dois bancos de dados distintos, o projeto segue organizado com base no padr�o **CQRS**.  
- O banco de dados escolhido foi o **SQL Server**, utilizando o **Entity Framework** para a persist�ncia.  

---

# Refinamento  
- **Por que um projeto s� pode ter 20 tarefas?**  
  Isso limita o planejamento antecipado de POs/PMs, entre outros. Talvez um limite configur�vel durante a cria��o do projeto seja mais eficaz. Outra op��o seria limitar o n�mero de tarefas por progresso, como, por exemplo, um m�ximo de 10 tarefas no status "Fazendo".  

- **Restri��es para alterar a prioridade das tarefas ap�s a cria��o:**  
  Essa restri��o pode impactar negativamente ambientes altamente din�micos. Uma solu��o seria tornar essa regra configur�vel ou definir condi��es espec�ficas, como restringir a altera��o apenas em determinados status, mas n�o em todos.  

- **Dele��o de projetos/tarefas:**  
  A dele��o do dado ser� f�sica ou l�gica (remover o registro ou so esconde-lo)?  

- **Hist�rico de altera��es:**  
  O hist�rico ser� utilizado apenas para auditoria ou haver� alguma regra de neg�cio associada a ele no futuro?  

- **Relat�rios e dados importantes:**  
  Quais dados s�o essenciais para o neg�cio? Os relat�rios ser�o espec�ficos para cada situa��o ou haver� um **dashboard** que permita filtragem? A solicita��o de relat�rios est� muito gen�rica: n�o define quantos relat�rios ser�o gerados nem o formato (Excel, PDF, dashboard).  

- **Valida��es de entrada e erros:**  
  � importante solicitar ao PO uma lista de erros poss�veis (valida��es de entrada e regras de neg�cio, como campos nulos, limites de caracteres, etc.) para melhorar o retorno ao usu�rio. Caso o PO n�o forne�a essa lista, eu realizaria as valida��es e encaminharia ao PO uma lista dos poss�veis erros, para que ele e o time de UI/UX validem as mensagens.  

- **Servi�o de autentica��o externo:**  
  O servi�o de autentica��o retorna o ID e o perfil do usu�rio, correto? Precisamos validar como esses dados ser�o mapeados no nosso sistema (necessidade de um de-para).  

- **Mensagens de erro:**  
  Ser� exibida apenas uma mensagem de erro por vez ou todas de uma vez?  

---

# Final  
- **Agregador de mensagens de erro:**  
  Pode ser necess�rio criar um agregador para facilitar o retorno de erros ao usu�rio. Uma solu��o seria implementar uma classe de validador com dois modos: agregador e �nico, dependendo da necessidade. Sugiro um padr�o semelhante ao do FluentValidation (mas sem utiliz�-lo diretamente). Um exemplo pode ser encontrado [neste c�digo](https://github.com/matheus-fiebig/functional-validation-poc/blob/main/Application/Customers/Services/CustomerService.cs), no m�todo `CreateCustomer`.  

- **Logs:**  
  Embora este projeto ainda n�o contenha logs, � essencial inclu�-los antes do GoLive.  

- **Soft delete vs. hard delete:**  
  Considerar soft delete pode ser necess�rio, dependendo das decis�es do PO.  

- **Divis�o de contextos (Query e Command):**  
  Pode ser �til caso o projeto cres�a e seja necess�rio utilizar dois bancos (um para leitura e outro para escrita). Entretanto, neste momento inicial, essa abordagem pode gerar custos desnecess�rios e n�o ser priorit�ria para um MVP.  

- **Otimiza��es de performance:**  
  Utilizar `AsNoTracking()` e configurar seu uso adequadamente para otimizar a performance.  

- **Configura��es sens�veis:**  
  Recomenda-se utilizar um **Key Vault** ou vari�veis de ambiente para armazenar configura��es do `appsettings`.  

- **Microservi�os e hist�rico:**  
  Caso seja um microservi�o, pode ser interessante criar uma **Minimal API** ou um **worker** exclusivo para salvar o hist�rico. Por�m, isso implica em custos adicionais e deve ser avaliado se realmente � necess�rio.  

- **Escalabilidade:**  
  Para melhorar a escalabilidade, pode ser interessante organizar os projetos por feature. Por exemplo, ao inv�s de utilizar uma estrutura como `EclipseWorks` e `EclipseWorks.Application`, seria mais eficiente algo como `Task.EclipseWorks` e `Task.EclipseWorks.Application`. Assim, no futuro, seria mais f�cil separar partes do projeto sem causar grandes impactos.
