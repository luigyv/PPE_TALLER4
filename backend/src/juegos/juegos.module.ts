import { Module } from '@nestjs/common';
import { JuegosService } from './juegos.service';
import { JuegosController } from './juegos.controller';

@Module({
  controllers: [JuegosController],
  providers: [JuegosService],
})
export class JuegosModule {}
