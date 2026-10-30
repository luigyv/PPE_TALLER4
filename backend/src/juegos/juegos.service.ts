import { Injectable, NotFoundException } from '@nestjs/common';
import { PrismaService } from '../prisma/prisma.service';
import { CreateJuegoDto } from './dto/create-juego.dto';
import { UpdateJuegoDto } from './dto/update-juego.dto';

interface FindAllParams {
  search?: string;
  page?: number;
  limit?: number;
}

@Injectable()
export class JuegosService {
  constructor(private readonly prisma: PrismaService) {}

  // GET /juegos?search=&page=1&limit=10
  // Devuelve exactamente la forma acordada en API_CONTRACT.md:
  // { data, total, page, limit }
  async findAll({ search = '', page = 1, limit = 10 }: FindAllParams) {
    const where = search
      ? { nombre: { contains: search } } // SQLite + Prisma: contains ya no distingue mayúsculas por defecto en collation binaria estándar
      : {};

    const [data, total] = await Promise.all([
      this.prisma.juego.findMany({
        where,
        skip: (page - 1) * limit,
        take: limit,
        orderBy: { id: 'asc' },
      }),
      this.prisma.juego.count({ where }),
    ]);

    return { data, total, page, limit };
  }

  // GET /juegos/:id
  async findOne(id: number) {
    const juego = await this.prisma.juego.findUnique({ where: { id } });
    if (!juego) {
      throw new NotFoundException(`No existe un juego con id ${id}`);
    }
    return juego;
  }

  // POST /juegos
  async create(dto: CreateJuegoDto) {
    return this.prisma.juego.create({ data: dto });
  }

  // PATCH /juegos/:id
  async update(id: number, dto: UpdateJuegoDto) {
    await this.findOne(id); // lanza 404 si no existe, antes de intentar actualizar
    return this.prisma.juego.update({ where: { id }, data: dto });
  }

  // DELETE /juegos/:id
  async remove(id: number) {
    await this.findOne(id); // lanza 404 si no existe
    await this.prisma.juego.delete({ where: { id } });
  }
}
