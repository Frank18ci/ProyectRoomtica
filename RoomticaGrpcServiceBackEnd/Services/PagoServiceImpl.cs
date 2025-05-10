using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class PagoServiceImpl : PagoService.PagoServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<PagoServiceImpl> _logger;

        public PagoServiceImpl(IConfiguration configuration, ILogger<PagoServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public override Task<Pagos> GetAll(Empty request, ServerCallContext context)
        {
            List<PagoDTO> lista = new List<PagoDTO>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_pagos", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new PagoDTO
                    {
                        Id = dr.GetInt32(0),
                        IdReserva = dr.GetDecimal(1).ToString(),
                        IdTipoComprobante = dr.GetString(2),
                        Igv = Double.Parse(dr.GetDecimal(3).ToString()),
                        TotalPago = Double.Parse(dr.GetDecimal(4).ToString()),
                        FechaEmision = dr.GetString(5),
                        FechaPago = dr.GetString(6),
                    });
                }
                dr.Close();
            }
            var pagos = new Pagos();
            pagos.Pagos_.AddRange(lista);
            return Task.FromResult(pagos);
        }

        public override Task<PagoDTO> GetByIdDTO(PagoId request, ServerCallContext context)
        {
            PagoDTO? pagoDTO = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_pagoDTO_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    pagoDTO = new PagoDTO
                    {
                        Id = dr.GetInt32(0),
                        IdReserva = dr.GetDecimal(1).ToString(),
                        IdTipoComprobante = dr.GetString(2),
                        Igv = Double.Parse(dr.GetDecimal(3).ToString()),
                        TotalPago = Double.Parse(dr.GetDecimal(4).ToString()),
                        FechaEmision = dr.GetString(5),
                        FechaPago = dr.GetString(6),
                        Estado = dr.GetBoolean(7)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(pagoDTO);
        }

        public override Task<Pago> GetById(PagoId request, ServerCallContext context)
        {
            Pago? pago = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_pago_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    pago = new Pago
                    {
                        Id = dr.GetInt32(0),
                        IdReserva = dr.GetInt32(1),
                        IdTipoComprobante = dr.GetInt32(2),
                        Igv = Double.Parse(dr.GetDecimal(3).ToString()),
                        TotalPago = Double.Parse(dr.GetDecimal(4).ToString()),
                        FechaEmision = dr.GetString(5),
                        FechaPago = dr.GetString(6),
                        Estado = dr.GetBoolean(7)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(pago);
        }

        public override Task<Pago> Create(Pago request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_pago", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_reserva", request.IdReserva);
                cmd.Parameters.AddWithValue("@id_tipo_comprobante", request.IdTipoComprobante);
                cmd.Parameters.AddWithValue("@igv", request.Igv);
                cmd.Parameters.AddWithValue("@total_pago", request.TotalPago);
                cmd.Parameters.AddWithValue("@fecha_emision", request.FechaEmision);
                cmd.Parameters.AddWithValue("@fecha_pago", request.FechaPago);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }

        public override Task<Pago> Update(Pago request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_pago", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@id_reserva", request.IdReserva);
                cmd.Parameters.AddWithValue("@id_tipo_comprobante", request.IdTipoComprobante);
                cmd.Parameters.AddWithValue("@igv", request.Igv);
                cmd.Parameters.AddWithValue("@total_pago", request.TotalPago);
                cmd.Parameters.AddWithValue("@fecha_emision", request.FechaEmision);
                cmd.Parameters.AddWithValue("@fecha_pago", request.FechaPago);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(PagoId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_pago", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
