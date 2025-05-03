using RoomticaGrpcServiceBackEnd.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<CaracteristicaHabitacionImpl>();
app.MapGrpcService<TipoHabitacionServiceImpl>();
app.MapGrpcService<UnidadMedidaProductoImpl>();
app.MapGrpcService<ProductoImpl>();
app.MapGrpcService<CategoriaProductoImpl>();
app.MapGrpcService<RolTrabajadorImpl>();
app.MapGrpcService<TipoDocumentoImpl>();
app.MapGrpcService<TipoNacionalidadImpl>();
app.MapGrpcService<TipoSexoImpl>();


app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
