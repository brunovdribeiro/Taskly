FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 9090

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

# Install the agent
RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
&& echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& wget https://download.newrelic.com/548C16BF.gpg \
&& apt-key add 548C16BF.gpg \
&& apt-get update \
&& apt-get install -y 'newrelic-dotnet-agent' \
&& rm -rf /var/lib/apt/lists/*

# Enable the agent
ENV CORECLR_ENABLE_PROFILING=1 \
OTEL_SERVICE_NAME=taskly \
OTEL_EXPORTER_OTLP_ENDPOINT=https://otlp.eu01.nr-data.net \
OTEL_EXPORTER_OTLP_HEADERS={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
OTEL_ATTRIBUTE_VALUE_LENGTH_LIMIT=4095 \
OTEL_EXPORTER_OTLP_COMPRESSION=gzip \
OTEL_EXPORTER_OTLP_PROTOCOL=http/protobuf \
OTEL_EXPORTER_OTLP_METRICS_TEMPORALITY_PREFERENCE=delta \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
NEW_RELIC_DISTRIBUTED_TRACING_ENABLED=true \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
NEW_RELIC_LICENSE_KEY=eu01xx4bdee6c2375ed3683b04cc3a13FFFFNRAL \
NEW_RELIC_APP_NAME="taskly"

# Install Node.js
RUN apt-get update && apt-get install -y \
    curl \
    && curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs \
    && npm install -g npm@latest \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /src
COPY ["src/Api/Api.csproj", "Api/"]
COPY ["src/Application/Application.csproj", "Application/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Api/Api.csproj"

COPY ["src/Api/", "Api/"]
COPY ["src/Application/", "Application/"]
COPY ["src/Domain/", "Domain/"]
COPY ["src/Infrastructure/", "Infrastructure/"]
COPY ["src/clientapp/app/", "clientapp/app/"]

# Build and publish .NET app
WORKDIR "/src/Api"
RUN dotnet publish "Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Build Next.js app
WORKDIR /src/clientapp/app
RUN npm install
RUN npm run build

# Create wwwroot and copy Next.js build output
RUN mkdir -p /app/publish/wwwroot
RUN cp -r .next/static /app/publish/wwwroot/
RUN cp -r .next/* /app/publish/wwwroot/

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Api.dll"]