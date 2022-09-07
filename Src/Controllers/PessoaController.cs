using Dio.DevWeekPottencial.Api.Src.Data;
using Dio.DevWeekPottencial.Api.Src.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Dio.DevWeekPottencial.Api.Src.Controllers
{
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<Pessoa> _logger;

        public PessoaController(DataContext context, ILogger<Pessoa> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region [ Pessoa ]
        /// <summary>
        /// Obtem a listagem de todas as pessoas
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/v1/pessoas")]
        public ActionResult<List<Pessoa>> GetAll()
        {
            try
            {
                List<Pessoa> listaPessoas = _context.Pessoas.Include(p => p.ListaContratos).ToList();
                if (listaPessoas == null)
                    return NoContent();
                return Ok(listaPessoas);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Title = "Erro",
                    Msg = $"Erro ao listar registros!",
                    Status = HttpStatusCode.InternalServerError
                });
            }
        }

        /// <summary>
        /// Obtem um registro de pessoa por meio do parametro [id] pela rota.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/v1/pessoa/{id:int}")]
        public ActionResult<Pessoa> GetById([FromRoute] int id)
        {
            try
            {
                Pessoa pessoa = _context.Pessoas.Include(p => p.ListaContratos).FirstOrDefault(p => p.Id == id);
                if (pessoa == null) return NotFound(new
                {
                    Title = "Erro",
                    Msg = "Registro não localizado!",
                    Status = HttpStatusCode.NotFound
                });

                return Ok(pessoa);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Title = "Erro",
                    Msg = $"Erro ao buscar registro {id}!",
                    Status = HttpStatusCode.InternalServerError
                });
            }
        }

        /// <summary>
        /// Cria um novo registro de pessoa, recebendo os dados no Body do request.
        /// </summary>
        /// <param name="pessoa"></param>
        /// <returns></returns>
        [HttpPost("api/v1/pessoa")]
        public ActionResult<Pessoa> Post([FromBody] Pessoa pessoa)
        {
            try
            {
                _context.Pessoas.Add(pessoa);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { Id = pessoa.Id }, pessoa);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Title = "Erro",
                    Msg = "Erro ao cadastrar registro!",
                    Status = HttpStatusCode.InternalServerError
                });
            }
        }

        /// <summary>
        /// Atualização de registro, recebendo o [id] pela rota e objeto novo no Body do request.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pessoa"></param>
        /// <returns></returns>
        [HttpPut("api/v1/pessoa/{id:int}")]
        public ActionResult Update([FromRoute] int id, [FromBody] Pessoa pessoa)
        {
            try
            {
                Pessoa pessoaToUpdate = _context.Pessoas.FirstOrDefault(p => p.Id == id);
                if (pessoaToUpdate == null) return NotFound(new
                {
                    Title = "Erro",
                    Msg = "Registro não localizado!",
                    Status = HttpStatusCode.NotFound
                });

                pessoaToUpdate.Nome = pessoa.Nome;
                pessoaToUpdate.Idade = pessoa.Idade;
                pessoaToUpdate.Cpf = pessoa.Cpf;
                pessoaToUpdate.Ativado = pessoa.Ativado;

                _context.Pessoas.Update(pessoaToUpdate);
                _context.SaveChanges();

                return Ok(new
                {
                    Title = "Sucesso",
                    Msg = "Atualização realizada com sucesso!",
                    Status = HttpStatusCode.OK
                });

            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Title = "Erro",
                    Msg = $"Erro ao processar atualização do registro {id}!",
                    Status = HttpStatusCode.InternalServerError
                });
            }
        }

        /// <summary>
        /// Exclusão de registro, recebendo [id] pela rota.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("api/v1/pessoa/{id:int}")]
        public ActionResult Delete([FromRoute] int id)
        {
            try
            {
                Pessoa pessoa = _context.Pessoas.FirstOrDefault(p => p.Id == id);

                if (pessoa == null) return BadRequest(new
                {
                    Title = "Erro",
                    Msg = "Registro não localizado!",
                    Status = HttpStatusCode.BadRequest
                });

                _context.Pessoas.Remove(pessoa);
                _context.SaveChanges();

                return Ok(new
                {
                    Title = "Sucesso",
                    Msg = $"Registro {id} removido com sucesso!",
                    Status = HttpStatusCode.OK
                });

            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Title = "Erro",
                    Msg = $"Erro ao deletar o registro {id}!",
                    Status = HttpStatusCode.InternalServerError
                });
            }
        }

        #endregion

        #region [ Contrato ]
        /// <summary>
        /// Obtem a listagem de todos os contratos de determinada pessoa, 
        /// [id] da pessoa recebido pela rota.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/v1/pessoa/{id:int}/contratos")]
        public ActionResult<List<Contrato>> GetAllContrato([FromRoute] int id)
        {
            try
            {
                Pessoa pessoa = _context.Pessoas.Include(p => p.ListaContratos).FirstOrDefault(p => p.Id == id);

                if (pessoa == null) return NotFound(new
                {
                    Title = "Erro",
                    Msg = "Pessoa não localizada!",
                    Status = HttpStatusCode.NotFound
                });

                if (pessoa.ListaContratos.Count == 0) return NotFound(new
                {
                    Title = "Erro",
                    Msg = "Registro não possui contratos!",
                    Status = HttpStatusCode.NotFound
                });

                return pessoa.ListaContratos;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Title = "Erro",
                    Msg = $"Erro ao listar registros de contrato!",
                    Status = HttpStatusCode.InternalServerError
                });
            }
        }

        /// <summary>
        /// Obtem um registro de contrato por meio dos parametros [pessoaId] e [contratoId] pela rota.
        /// </summary>
        /// <param name="pessoaId"></param>
        /// <param name="contratoId"></param>
        /// <returns></returns>
        [HttpGet("api/v1/pessoa/{pessoaId:int}/contrato/{contratoId:int}")]
        public ActionResult GetContratoById([FromRoute] int pessoaId, [FromRoute] int contratoId)
        {
            try
            {
                Pessoa pessoa = _context.Pessoas.Include(p => p.ListaContratos).FirstOrDefault(p => p.Id == pessoaId);
                if (pessoa == null) return NotFound(new
                {
                    Title = "Erro",
                    Msg = "Pessoa não localizada!",
                    Status = HttpStatusCode.NotFound
                });

                foreach (Contrato item in pessoa.ListaContratos)
                {
                    if (item.Id == contratoId)
                    {
                        Contrato contrato = new Contrato();
                        contrato = item;

                        return Ok(contrato);
                    }
                }
                return NotFound(new
                {
                    Title = "Erro",
                    Msg = "Contrato não localizado!",
                    Status = HttpStatusCode.BadRequest
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Title = "Erro",
                    Msg = "Erro ao buscar contratos!",
                    Status = HttpStatusCode.InternalServerError
                });
            }
        }

        /// <summary>
        /// Cria um registro de contrato, recebendo [pessoaId] pela rota e os dados [contrato] no Body do request.
        /// </summary>
        /// <param name="pessoaId"></param>
        /// <param name="contrato"></param>
        /// <returns></returns>
        [HttpPost("api/v1/pessoa/{pessoaId:int}/contrato")]
        public ActionResult PostContrato([FromRoute] int pessoaId, [FromBody] Contrato contrato)
        {
            try
            {
                Pessoa pessoa = _context.Pessoas.FirstOrDefault(p => p.Id == pessoaId);
                if (pessoa == null) return BadRequest(new
                {
                    Title = "Erro",
                    Msg = "Pessoa não localizada!",
                    Status = HttpStatusCode.NotFound
                });

                if (contrato.PessoaId != pessoaId) return BadRequest(new
                {
                    Title = "Erro",
                    Msg = "Dados da pessoa divergentes.",
                    Status = HttpStatusCode.BadRequest
                });

                _context.Contratos.Add(contrato);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetContratoById), new { pessoaId = pessoa.Id, contratoId = contrato.Id }, contrato);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Title = "Erro",
                    Msg = "Erro ao cadastrar contrato!",
                    Status = HttpStatusCode.InternalServerError
                });
            }
        }

        /// <summary>
        /// Atualiza um registro de contrato, recebendo o [pessoaId], [contratoId] por meio da rota
        /// e os novos dados por meio 
        /// </summary>
        /// <param name="pessoaId"></param>
        /// <param name="contratoId"></param>
        /// <param name="contrato"></param>
        /// <returns></returns>
        [HttpPut("api/v1/pessoa/{pessoaId:int}/contrato/{contratoId:int}")]
        public ActionResult UpdateContrato([FromRoute] int pessoaId, [FromRoute] int contratoId, [FromBody] Contrato contrato)
        {
            try
            {
                if (pessoaId != contrato.PessoaId) return BadRequest(new
                {
                    Title = "Erro",
                    Msg = "Dados da pessoa divergentes.",
                    Status = HttpStatusCode.BadRequest
                });

                if (contratoId != contrato.Id) return BadRequest(new
                {
                    Title = "Erro",
                    Msg = "Dados do contrato divergentes.",
                    Status = HttpStatusCode.BadRequest
                });

                Pessoa pessoa = _context.Pessoas.Include(p => p.ListaContratos).FirstOrDefault(p => p.Id == pessoaId);
                if (pessoa == null) return NotFound(new
                {
                    Title = "Erro",
                    Msg = "Pessoa não localizada!",
                    Status = HttpStatusCode.NotFound
                });

                foreach (Contrato item in pessoa.ListaContratos)
                {
                    if (item.Id == contratoId)
                    {
                        Contrato contratoToUpdate = new Contrato();
                        contratoToUpdate = item;

                        contratoToUpdate.Pago = contrato.Pago;

                        _context.Contratos.Update(contratoToUpdate);
                        _context.SaveChanges();
                        return RedirectToAction("GetContratoById", new { pessoaId = pessoa.Id, contratoId = item.Id });
                    }
                }

                return NotFound(new
                {
                    Title = "Erro",
                    Msg = "Contrato não localizado!",
                    Status = HttpStatusCode.BadRequest
                });

            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Title = "Erro",
                    Msg = "Erro ao atualizar contratos!",
                    Status = HttpStatusCode.InternalServerError
                });
            }
        }

        /// <summary>
        /// Deleta um registro de contrato, recebendo o [pessoaId], [contratoId] pela rota
        /// e para confirmação os dados do contrato no Body do Request.
        /// </summary>
        /// <param name="pessoaId"></param>
        /// <param name="contratoId"></param>
        /// <param name="contrato"></param>
        /// <returns></returns>
        [HttpDelete("api/v1/pessoa/{pessoaId:int}/contrato/{contratoId}")]
        public ActionResult DeleteContrato([FromRoute] int pessoaId, [FromRoute] int contratoId, [FromBody] Contrato contrato)
        {
            try
            {
                if (pessoaId != contrato.PessoaId) return BadRequest(new
                {
                    Title = "Erro",
                    Msg = "Dados da pessoa divergentes.",
                    Status = HttpStatusCode.BadRequest
                });

                if (contratoId != contrato.Id) return BadRequest(new
                {
                    Title = "Erro",
                    Msg = "Dados do contrato divergentes.",
                    Status = HttpStatusCode.BadRequest
                });

                Pessoa pessoa = _context.Pessoas.Include(p => p.ListaContratos).FirstOrDefault(p => p.Id == pessoaId);
                if (pessoa == null) return NotFound(new
                {
                    Title = "Erro",
                    Msg = "Pessoa não localizada!",
                    Status = HttpStatusCode.NotFound
                });

                foreach (Contrato item in pessoa.ListaContratos)
                {
                    if (item.Id == contratoId)
                    {
                        _context.Contratos.Remove(item);
                        _context.SaveChanges();

                        return Ok(new
                        {
                            Title = "Sucesso",
                            Msg = $"Contrato {contratoId} removido com sucesso!",
                            Status = HttpStatusCode.OK
                        });
                    }
                }

                return NotFound(new
                {
                    Title = "Erro",
                    Msg = "Contrato não localizado!",
                    Status = HttpStatusCode.BadRequest
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Title = "Erro",
                    Msg = "Erro ao Deletar o registro de contrato!",
                    Status = HttpStatusCode.InternalServerError
                });
            }
        }
        #endregion
    }
}
