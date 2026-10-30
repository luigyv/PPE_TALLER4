import { NestFactory } from '@nestjs/core';
import { ValidationPipe } from '@nestjs/common';
import { AppModule } from './app.module';

async function bootstrap() {
  const app = await NestFactory.create(AppModule);

  // Sin esto, Vue (en localhost:5173) no puede llamar a este backend (localhost:3000)
  app.enableCors();

  // Aplica automáticamente las validaciones de los DTOs (class-validator)
  // y descarta cualquier campo que no esté declarado en el DTO.
  app.useGlobalPipes(
    new ValidationPipe({
      whitelist: true,
      transform: true,
    }),
  );

  await app.listen(process.env.PORT ?? 3000);
}
bootstrap();
