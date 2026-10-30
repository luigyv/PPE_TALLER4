import { IsNotEmpty, IsString } from 'class-validator';

export class CreateJuegoDto {
  @IsString()
  @IsNotEmpty({ message: 'El nombre es obligatorio' })
  nombre: string;

  @IsString()
  @IsNotEmpty({ message: 'La imagen (URL) es obligatoria' })
  imagen: string;

  @IsString()
  @IsNotEmpty({ message: 'La categoría es obligatoria' })
  categoria: string;
}
