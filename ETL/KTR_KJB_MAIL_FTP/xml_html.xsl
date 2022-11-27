<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
 
    <!--tipo de output: text, html, xml-->
    <xsl:output method="html" indent="yes"/>

    <xsl:template match="/">
        <html>
            <head>
                <title>Quadro de Mérito | Excelência <xsl:value-of select="alunos/aluno/ano/text()"/>º ano</title>
                <link rel="stylesheet" type="text/css" href="estilos.css"/>
            </head>
            <body>
                <div id="banner">
                    <img src="banner.jpg" width="700" height="115"/>
                </div>
                <table class="header">
                
                    <tr>
                        <th scope="col" colspan="8">Quadros de Mérito | Excelência 2013|2014</th>
                    </tr>
                    <tr>
                        <td class="links">
                            <a href="5Ano.html">5º</a>
                        </td>
                        <td class="links">
                            <a href="6Ano.html">6º</a>
                        </td>
                        <td class="links">
                            <a href="7Ano.html">7º</a>
                        </td>
                        <td class="links">
                            <a href="8Ano.html">8º</a>
                        </td>
                        <td class="links">
                            <a href="9Ano.html">9º</a>
                        </td>
                        <td class="links">
                            <a href="10Ano.html">10º</a>
                        </td>
                        <td class="links">
                            <a href="11Ano.html">11º</a>
                        </td>
                        <td class="links">
                            <a href="12Ano.html">12º</a>
                        </td>
                    </tr>
                    <tr>
                        <th colspan="8" class="ano"><xsl:value-of select="alunos/aluno/ano/text()"/>ºano</th>
                    </tr>
                    
                </table>
                <table class="alunos">
                    <xsl:call-template name="alunos"/>
                </table>
            </body>
        </html>
    </xsl:template>
    
		<!-- Para as turmas de A a M, vamos carrega-las com o numero de ocorrencias do respetivo valor:-->
		<!-- Ou seja, contamos todas as ocorrencias do valor do elemento <turma> igual da "A", "B" -->
		<!-- Cada variavel por turma, toma o valor do somatorio das anteriores + o total das suas ocorrencias-->
		<!-- Indicamos como limite de turmas a letra M, que dificilmente ocorrera-->
    <xsl:template name="alunos">
        <xsl:variable name="turmaA">
            <xsl:value-of select="count(alunos/aluno/turma[text()='A'])" />
        </xsl:variable>
        <xsl:variable name="turmaB">
            <xsl:value-of select="$turmaA+count(alunos/aluno/turma[text()='B'])" />
        </xsl:variable>
        <xsl:variable name="turmaC">
            <xsl:value-of select="$turmaB+count(alunos/aluno/turma[text()='C'])" />
        </xsl:variable>
        <xsl:variable name="turmaD">
            <xsl:value-of select="$turmaC+count(alunos/aluno/turma[text()='D'])" />
        </xsl:variable>
        <xsl:variable name="turmaE">
            <xsl:value-of select="$turmaD+count(alunos/aluno/turma[text()='E'])" />
        </xsl:variable>
        <xsl:variable name="turmaF">
            <xsl:value-of select="$turmaE+count(alunos/aluno/turma[text()='F'])" />
        </xsl:variable>
        <xsl:variable name="turmaG">
            <xsl:value-of select="$turmaF+count(alunos/aluno/turma[text()='G'])" />
        </xsl:variable>
        <xsl:variable name="turmaH">
            <xsl:value-of select="$turmaG+count(alunos/aluno/turma[text()='H'])" />
        </xsl:variable>
        <xsl:variable name="turmaI">
            <xsl:value-of select="$turmaH+count(alunos/aluno/turma[text()='I'])" />
        </xsl:variable>
        <xsl:variable name="turmaJ">
            <xsl:value-of select="$turmaI+count(alunos/aluno/turma[text()='J'])" />
        </xsl:variable>
        <xsl:variable name="turmaK">
            <xsl:value-of select="$turmaJ+count(alunos/aluno/turma[text()='K'])" />
        </xsl:variable>
        <xsl:variable name="turmaL">
            <xsl:value-of select="$turmaK+count(alunos/aluno/turma[text()='L'])" />
        </xsl:variable>
        <xsl:variable name="turmaM">
            <xsl:value-of select="$turmaL+count(alunos/aluno/turma[text()='M'])" />
        </xsl:variable>
            
        <tr>
            <th scope="col">Nº Aluno</th>
            <th scope="col">Nome</th>
            <th scope="col">Média</th>
            <th scope="col">Classificação</th>
        </tr>            

        <xsl:for-each select="alunos/aluno">
            <xsl:sort select="turma" order="ascending"/>
            <xsl:sort select="media" order="ascending"/>
            
            <!-- carrega a variavel com a turma da linha processada -->
            <xsl:variable name="var_turma">
                <xsl:value-of select="turma/text()"/>
            </xsl:variable>
            
            <!-- por cada linha processada, verifica se a turma carregada é igual a 'A', 'B'... -->
            <!-- caso haja correspondencia, verifica se esta na 1ª posicao desse grupo --> 
            <xsl:if test="$var_turma = 'A'">
                <xsl:if test="position() &lt; 2">
                    <tr>
                        <th colspan="4">Turma A</th>
                    </tr>
                </xsl:if>
                <tr>
                    <td>
                        <xsl:value-of select="@nprocesso"/>
                    </td>                            
                    <td>
                        <xsl:value-of select="nome/text()"/>
                    </td>
                    <td>
                        <xsl:value-of select="media/text()"/>
                    </td>   
                    <td>
                        <xsl:value-of select="classificacao/text()"/>
                    </td>                         
                </tr>
            </xsl:if>
            <xsl:if test="$var_turma = 'B'">
                <!-- cada variavel por turma tem o somatorio das anteriores-->
                <!-- a 1ª posicao de cada turma no XML e o somatorio das anteriores + 1 -->
                <xsl:if test="position() = ($turmaA+1)">
                    <tr>
                        <th colspan="4">Turma B</th>
                    </tr>
                </xsl:if>
                <tr>
                    <td>
                        <xsl:value-of select="@nprocesso"/>
                    </td>                            
                    <td>
                        <xsl:value-of select="nome/text()"/>
                    </td>
                    <td>
                        <xsl:value-of select="media/text()"/>
                    </td>   
                    <td>
                        <xsl:value-of select="classificacao/text()"/>
                    </td>                         
                </tr>
            </xsl:if>
            <xsl:if test="$var_turma = 'C'">
                <xsl:if test="position() = ($turmaB+1)">
                    <tr>
                        <th colspan="4">Turma C</th>
                    </tr>
                </xsl:if>
                <tr>
                    <td>
                        <xsl:value-of select="@nprocesso"/>
                    </td>                            
                    <td>
                        <xsl:value-of select="nome/text()"/>
                    </td>
                    <td>
                        <xsl:value-of select="media/text()"/>
                    </td>   
                    <td>
                        <xsl:value-of select="classificacao/text()"/>
                    </td>                         
                </tr>
            </xsl:if>
            <xsl:if test="$var_turma = 'D'">
                <xsl:if test="position() = ($turmaC+1)">
                    <tr>
                        <th colspan="4">Turma D</th>
                    </tr>
                </xsl:if>
                <tr>
                    <td>
                        <xsl:value-of select="@nprocesso"/>
                    </td>                            
                    <td>
                        <xsl:value-of select="nome/text()"/>
                    </td>
                    <td>
                        <xsl:value-of select="media/text()"/>
                    </td>   
                    <td>
                        <xsl:value-of select="classificacao/text()"/>
                    </td>                         
                </tr>
            </xsl:if>
            <xsl:if test="$var_turma = 'E'">
                <xsl:if test="position() = ($turmaD+1)">
                    <tr>
                        <th colspan="4">Turma E</th>
                    </tr>
                </xsl:if>
                <tr>
                    <td>
                        <xsl:value-of select="@nprocesso"/>
                    </td>                            
                    <td>
                        <xsl:value-of select="nome/text()"/>
                    </td>
                    <td>
                        <xsl:value-of select="media/text()"/>
                    </td>   
                    <td>
                        <xsl:value-of select="classificacao/text()"/>
                    </td>                         
                </tr>
            </xsl:if>
            <xsl:if test="$var_turma = 'F'">
                <xsl:if test="position() = ($turmaE+1)">
                    <tr>
                        <th colspan="4">Turma F</th>
                    </tr>
                </xsl:if>
                <tr>
                    <td>
                        <xsl:value-of select="@nprocesso"/>
                    </td>                            
                    <td>
                        <xsl:value-of select="nome/text()"/>
                    </td>
                    <td>
                        <xsl:value-of select="media/text()"/>
                    </td>   
                    <td>
                        <xsl:value-of select="classificacao/text()"/>
                    </td>                         
                </tr>
            </xsl:if>
            <xsl:if test="$var_turma = 'G'">
                <xsl:if test="position() = ($turmaF+1)">
                    <tr>
                        <th colspan="4">Turma G</th>
                    </tr>
                </xsl:if>
                <tr>
                    <td>
                        <xsl:value-of select="@nprocesso"/>
                    </td>                            
                    <td>
                        <xsl:value-of select="nome/text()"/>
                    </td>
                    <td>
                        <xsl:value-of select="media/text()"/>
                    </td>   
                    <td>
                        <xsl:value-of select="classificacao/text()"/>
                    </td>                         
                </tr>
            </xsl:if>
            <xsl:if test="$var_turma = 'H'">
                <xsl:if test="position() = ($turmaG+1)">
                    <tr>
                        <th colspan="4">Turma H</th>
                    </tr>
                </xsl:if>
                <tr>
                    <td>
                        <xsl:value-of select="@nprocesso"/>
                    </td>                            
                    <td>
                        <xsl:value-of select="nome/text()"/>
                    </td>
                    <td>
                        <xsl:value-of select="media/text()"/>
                    </td>   
                    <td>
                        <xsl:value-of select="classificacao/text()"/>
                    </td>                         
                </tr>
            </xsl:if>
            <xsl:if test="$var_turma = 'I'">
                <xsl:if test="position() = ($turmaH+1)">
                    <tr>
                        <th colspan="4">Turma I</th>
                    </tr>
                </xsl:if>
                <tr>
                    <td>
                        <xsl:value-of select="@nprocesso"/>
                    </td>                            
                    <td>
                        <xsl:value-of select="nome/text()"/>
                    </td>
                    <td>
                        <xsl:value-of select="media/text()"/>
                    </td>   
                    <td>
                        <xsl:value-of select="classificacao/text()"/>
                    </td>                         
                </tr>
            </xsl:if>                    
        </xsl:for-each>
    </xsl:template>
</xsl:stylesheet>