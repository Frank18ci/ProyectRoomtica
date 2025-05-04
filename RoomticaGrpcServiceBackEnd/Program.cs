using RoomticaGrpcServiceBackEnd.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
//app.MapGrpcService<GreeterService>();
app.MapGrpcService<CaracteristicaHabitacionServiceImpl>();
app.MapGrpcService<CaracteristicaHabitacionTipoHabitacionServiceImpl>();
app.MapGrpcService<CategoriaProductoServiceImpl>();
app.MapGrpcService<ClienteServiceImpl>();
app.MapGrpcService<ConsumoServiceImpl>();
app.MapGrpcService<EstacionamientoServiceImpl>();
app.MapGrpcService<EstadoHabitacionServiceImpl>();
app.MapGrpcService<HabitacionServiceImpl>();
app.MapGrpcService<PagoServiceImpl>();
app.MapGrpcService<ProductoServiceImpl>();
app.MapGrpcService<ReservaEstacionamientoServiceImpl>();
app.MapGrpcService<ReservaServiceImpl>();
app.MapGrpcService<RolTrabajadorServiceImpl>();
app.MapGrpcService<TipoComprobanteServiceImpl>();
app.MapGrpcService<TipoDocumentoServiceImpl>();
app.MapGrpcService<TipoEstacionamientoServiceImpl>();
app.MapGrpcService<TipoHabitacionServiceImpl>();
app.MapGrpcService<TipoNacionalidadServiceImpl>();
app.MapGrpcService<TipoReservaServiceImpl>();
app.MapGrpcService<TipoSexoServiceImpl>();
app.MapGrpcService<TrabajadorServiceImpl>();
app.MapGrpcService<UnidadMedidaProductoServiceImpl>();



app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
