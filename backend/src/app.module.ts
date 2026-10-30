import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { PrismaModule } from './prisma/prisma.module';
import { JuegosModule } from './juegos/juegos.module';

@Module({
  imports: [PrismaModule, JuegosModule],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
